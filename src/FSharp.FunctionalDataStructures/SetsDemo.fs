module SetsDemo

open System.Text
open System.Security.Cryptography

let animals = ["dog"; "cat"; "pigeon"] |> Set.ofList // or Set([...])
// new set
let animals' = animals.Add "monkey"
// unchanged set
let animals'' = animals'.Add "dog"

// operators:
    // Set.union or setA + setB
    // Set.difference or setA - setB
    // Set.isSubset
    // Set.isSuperset

// hidden mutability
let secretPassword = "password123"

let stringMD5(str : string) =
    use md5 = MD5.Create()
    md5.ComputeHash(Encoding.UTF8.GetBytes(str))

let passwordAttempts =
    let attempts = ref Set.empty
    (fun (passwordHash) ->
        attempts := Set.add passwordHash !attempts 
        (!attempts).Count)

let checkPassword password =
    let attempts = passwordAttempts (stringMD5 password)
    if (attempts > 3) then
        "Too many tries"
    else 
        "Try again"