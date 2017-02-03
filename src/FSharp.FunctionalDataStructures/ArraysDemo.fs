module ArraysDemo

open System
open System.Net 

let requests = 
    [|
        "http://www.google.com"
        "http://www.pluralsight.com"
        "http://99.99.99.99/non-existant"
    |]

let GetRequests(requests : string[]) =
    use wc = new WebClient()

    requests
    |> Array.map(fun url ->
        try
            wc.DownloadString url |> Some
        with
        | _ -> None)
    |> Array.filter(fun s -> s.IsSome)
    |> Array.map(fun s -> s.Value)

let GetRequestsChoose(requests : string[]) =
    use wc = new WebClient()

    requests
    |> Array.choose(fun url ->
        try
            wc.DownloadString url |> Some
        with
        | _ -> None)

requests
|> GetRequests 
|> Array.iter(fun s -> printfn "Content: %s" (s.Trim().Substring(0, 100)))

requests
|> GetRequestsChoose
|> Array.iter(fun s -> printfn "Content: %s" (s.Trim().Substring(0, 100)))
