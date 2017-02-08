// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open System.IO
open Argu

type Label = Spam | Ham

type Corpus = {
    spamFreqs: Map<string, int>;
    hamFreqs: Map<string, int>;
    spamCount: int;
    hamCount: int;
    totalCount: int;
}

let messagesPath = Path.Combine(__SOURCE_DIRECTORY__, "SpamCollection.txt")

let string2Label str =
    match str with
    | "ham" -> Ham
    | _ -> Spam

let makeRandomPredicate fractionTrue =
    let r = Random()

    let predicate x = 
        r.NextDouble() < fractionTrue

    predicate

let loadMessages filepath =
    let contents = 
        File.ReadAllLines(filepath)
            |> Seq.map(fun l -> l.Split( [|'\t'|] ) )
            |> Seq.map(fun arr -> (string2Label arr.[0], arr.[1]) )

    contents

let partitionMessagesRandomly messages fraction =
    messages
    |> Seq.toList
    |> List.partition (makeRandomPredicate fraction)

let accuracyRate classifier labeledData =
    let correctlyClassified = 
        labeledData 
        |> Seq.sumBy(fun (label, msg) -> if (classifier msg) = label then 1.0 else 0.0)

    let total = 
        labeledData 
        |> Seq.length 
        |> float

    correctlyClassified / total

let features (msg:string) =
    // most naive thing we could think of
    msg.Split([|' '|]) |> Array.toSeq

let countLabel label (labeledWords:seq<Label*string>) =
    labeledWords 
    |> Seq.filter(fun (l, word) -> l = label) 
    |> Seq.length

let frequencies label (labeledWords:seq<Label*string>) =
    labeledWords 
    |> Seq.filter(fun (l, word) -> l = label)
    |> Seq.groupBy(fun (l, word) -> word)
    |> Seq.map(fun (w, ws) -> w, (Seq.length ws))
    |> Map.ofSeq

let getCount map word =
    match (Map.tryFind word map) with
    | Some x -> float x
    | _ -> 0.0001 // return a small, insignificant value

let pHam corpus =
    (float corpus.hamCount) / (float corpus.totalCount)

let pSpam corpus =
    (float corpus.spamCount) / (float corpus.totalCount)

let pWordGivenSpam word corpus =
    (getCount corpus.spamFreqs word) / (float corpus.spamCount)

let pWordGivenHam word corpus = 
    (getCount corpus.hamFreqs word) / (float corpus.hamCount)

let pWord word corpus =
    (pWordGivenSpam word corpus) * (pSpam corpus) + (pWordGivenHam word corpus) * (pHam corpus)

let pHamGivenWords words corpus =
    let product =
        words
        |> Seq.map (fun w -> (pWordGivenHam w corpus) / (pWord w corpus))
        |> Seq.reduce (*)

    (pHam corpus) * product

let pSpamGivenWords words corpus =
    let product =
        words
        |> Seq.map (fun w -> (pWordGivenSpam w corpus) / (pWord w corpus))
        |> Seq.reduce (*)

    (pSpam corpus) * product

let classify msg =
    Spam

let bayesClassifierFactory corpus =
    let bayesClassify msg =
        let words = features msg
        let pHam = pHamGivenWords words corpus
        let pSpam = pSpamGivenWords words corpus
        if pHam > pSpam then Ham else Spam

    bayesClassify

let createCorpus messages =
    let labeledWords =
        messages
        |> Seq.map(fun (label, msg) -> (label, features msg)) // get a features sequence from the message 
        |> Seq.collect(fun (label, words) -> 
            words |> Seq.map(fun w -> label, w)
        )

    {
        spamFreqs = frequencies Spam labeledWords
        hamFreqs = frequencies Ham labeledWords
        spamCount = countLabel Spam labeledWords
        hamCount = countLabel Ham labeledWords
        totalCount = Seq.length labeledWords
    }

type Arguments = 
    | [<Mandatory>] TrainingData of string
    | Message of string
    | Stats
with
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | TrainingData _ -> "The filename with the data to train the classifier"
            | Message _ -> "Message to classify"
            | Stats _ -> "Show the statistics for the classifier"

[<EntryPoint>]
let main argv = 
    let parser = ArgumentParser.Create<Arguments>()
    let results = parser.Parse(argv)

    let messagesPath = results.GetResult <@ TrainingData @>

    let messages = loadMessages messagesPath
    let corpus = createCorpus messages
    let bayesClassify = bayesClassifierFactory corpus

    if results.Contains <@ Message @> then
        let label = bayesClassify(results.GetResult <@ Message @>)
        printfn "%A" label

    if results.Contains <@ Stats @> then
        let trainingMessages, validationMessages = partitionMessagesRandomly messages 0.8
        let corpus = createCorpus trainingMessages
        let bayesClassify = bayesClassifierFactory corpus

        printfn "Testing message words: Total=%A, Ham=%A, Spam=%A" corpus.totalCount corpus.hamCount corpus.spamCount
        printfn "The bayes accuracy is %A" (accuracyRate bayesClassify validationMessages)

    0 // return an integer exit code
