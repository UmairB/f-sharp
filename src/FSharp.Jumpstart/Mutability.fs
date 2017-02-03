#if INTERACTIVE
#else
module Mutability
#endif

let MutabilityDemo() =
    let mutable x = 42
    printfn "x: %i" x
    x <- x + 1
    printfn "x: %i" x

let RefCellDemo() =
    let x = ref 42
    x := 43
    printfn "x: %i" !x

let PrintLines() =
    seq {
        let finished = ref false
        while not !finished do
            match System.Console.ReadLine() with
            | null -> finished := true
            | s -> yield s
    }
