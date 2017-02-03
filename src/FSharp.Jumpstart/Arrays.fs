#if INTERACTIVE
#else
module Arrays
#endif

let squares = [| for i in 0..2..99 do yield i * i |]

let RandomFruits count = 
    let r = System.Random()
    let fruits = [|"apple";"orange";"pear"|]
    let fruitsLength = fruits.Length
    [|
        for i in 1..count do
            let index = r.Next(fruitsLength)
            yield fruits.[index]
    |]

let RandomFruits2 count = 
    let r = System.Random()
    let fruits = [|"apple";"orange";"pear"|]
    let fruitsLength = fruits.Length
    Array.init count (fun _ ->
        let index = r.Next(fruitsLength)
        fruits.[index]
    )

let LikeSomeFruit fruits = 
    for fruit in fruits do
        printfn "I like %s" fruit

LikeSomeFruit (RandomFruits2 3)

let IsOrange fruit =
    fruit = "orange"

// original array is not changes by filtering
let oranges = Array.filter IsOrange (RandomFruits2 10)

// piping
let PrintLongWords(words: string[]) =
    let longWords : string[] = Array.filter (fun w -> w.Length > 8) words
    let sortedLongWords = Array.sort longWords
    Array.iter (fun w -> printfn "%s" w) sortedLongWords

let PrintLongWords2(words: string[]) =
    words 
    |> Array.filter (fun w -> w.Length > 8)
    |> Array.sort
    |> Array.iter (fun w -> printfn "%s" w)

// lists are immutable. Arrays are not
let PrintSquares min max =
    let square n = 
        n * n
    [min..max]
    |> List.map square
    |> List.iter (printfn "%i")

// sequences are constructs that implement IEnumerable
let smallNumbers = {0..99}
let smallNumbers2 = Seq.init 100 (fun i -> i)
let smallNumbers3 = seq {
    for i in 0..99 do
        yield i
}

let bigFiles = 
    System.IO.Directory.EnumerateFiles(@"c:\windows")
    |> Seq.map (fun path -> System.IO.FileInfo path)
    |> Seq.filter (fun fi -> fi.Length > 1000000L)
    |> Seq.map (fun fi -> fi.Name)
