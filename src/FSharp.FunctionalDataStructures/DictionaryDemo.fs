module DictionaryDemo

open System.IO
open System.Collections.Generic
open System.Security.Cryptography

type LatLong = { Lat : float; Long: float }

// .NET dictionary. Is mutable
let dictionary = new Dictionary<string, LatLong>()
// Assigning using <- Adds if the value doesn't exist and update otherwise
dictionary.["Key"] <- { Lat = 10.1; Long = 10.2 }

// dict operator/function. Returns immutable dictionary implementing IDictionary
let fileMD5 filepath =
    use md5 = MD5.Create()
    use stream = File.OpenRead(filepath)
    md5.ComputeHash(stream)

type FileExistsChecker(dirPath: string) =
    let md5Dict = 
        dirPath
        |> Directory.EnumerateFiles
        |> Seq.map(fun filepath -> fileMD5 filepath, filepath)
        |> dict

    member this.ExistingFile filepath =
        let md5 = fileMD5 filepath
        let ok, existingFilepath = md5Dict.TryGetValue(md5)
        if ok then
            existingFilepath |> Some
        else
            None
