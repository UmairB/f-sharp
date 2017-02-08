module Tests

open FsUnit
open NUnit.Framework
open FSharpTV.SimplePredictiveText.PredictiveText

let dictionary = LoadDefaultDict()

[<Test>]
let shouldNotBeEmpty() =
    dictionary |> should not' (be Empty)

[<Test>]
let shouldNotContainFSharp() =
    dictionary |> should not' (contain "fsharp")

[<Test>]
let shouldContainOneResultForAardvarks() =
    dictionary 
    |> Autocomplete "aardvarks"
    |> Array.length
    |> should equal 1
