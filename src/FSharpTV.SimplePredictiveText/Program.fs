open System
open FSharpTV.SimplePredictiveText.PredictiveText

[<EntryPoint>]
let main argv = 
    let dict = LoadDefaultDict()

    while true do
        printfn "Enter prefix: "
        let prefix = Console.ReadLine()

        let candidates = dict |> Autocomplete prefix
        if candidates.Length > 7 then
            printfn "Too many candidates found. Please narrow the search prefix."
        else
            candidates |> printfn "%A"

    0 // return an integer exit code
