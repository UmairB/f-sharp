module FSharpTV.SimplePredictiveText.PredictiveText

open System
open System.IO

let private dictPath = Path.Combine(__SOURCE_DIRECTORY__, "dict.txt")

/// Load dictionary from a file path
let LoadDict filepath =
    let contents = File.ReadAllLines(filepath)
    contents

/// Loads the default US dictionary
let LoadDefaultDict() =
    LoadDict dictPath

/// Search a dictionary for strings matching the prefix
let Autocomplete prefix (dict : string[]) =
    dict |> Array.filter(fun word -> word.StartsWith(prefix))
    