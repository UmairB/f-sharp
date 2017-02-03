#if INTERACTIVE
#else
module Types
#endif

// Records - immutable types
type Person =
    {
        FirstName : string
        LastName : string
    }

let person = { FirstName = "Kit"; LastName = "Eason" }

printfn "%s, %s" person.LastName person.FirstName

let person2 = { person with FirstName = "Christian" }
// records types are structurally equal so person = person3 (content equality)
let person3 = { FirstName = "Kit"; LastName = "Eason" }

type Company = 
    {
        Name : string
        SalesTaxNumber : int option
    }

let PrintCompany (company : Company) =
    let taxNumberString = 
        match company.SalesTaxNumber with
        | Some n -> sprintf " [%i]" n
        | None -> ""

    printfn "%s%s" company.Name taxNumberString

let company = { Name = "Kit Enterprises"; SalesTaxNumber = None }
let company2 = { Name = "Acme Systems"; SalesTaxNumber = Some 12345 }

// discriminated union types
type Shape = 
    | Square of float
    | Rectangle of float * float
    | Circle of float

let s = Square 3.4
let r = Rectangle(2.2, 1.9)
let c = Circle(1.0)

// can put different cases in the same collection as the type is the same
let drawing = [|s; r; c|]

let Area (shape: Shape) = 
    match shape with
    | Square x -> x * x
    | Rectangle(h, w) -> h * w
    | Circle r -> System.Math.PI * r * r

let totalArea = drawing |> Array.sumBy Area

let one = [|50|]
let two = [|60; 61|]
let many = [|0..99|]

let Describe arr =
    match arr with
    | [|x|] -> sprintf "One element: %i" x
    | [|x; y|] -> sprintf "Two elements: %i, %i" x y
    | _ -> sprintf "A longer array"
