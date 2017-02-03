#if INTERACTIVE
#else
module ObjectOrientedTypes
#endif

open System

type Person(forename : string, surname : string) =
    member this.Forename = forename
    member this.Surname = surname

type MutablePerson(forename : string, surname : string) =
    let mutable _forename = forename
    let mutable _surname = surname

    member this.Forename 
        with get() = _forename
        and set value = _forename <- value

    member this.Surname 
        with get() = _surname
        and set value = _surname <- value

type MutablePerson2(forename : string, surname : string) =
    member val Forename = forename with get, set
    member val Surname = surname with get, set

let person = MutablePerson2("Umair", "But")
person.Surname <- "Butt"

type IPerson =
    abstract member Forename : string
    abstract member Surname : string
    abstract member Fullname : string

type PersonFromInterface(forename : string, surname : string) =
    let validateString str = 
        if String.IsNullOrWhiteSpace str then
            raise (ArgumentException())
    do
        validateString forename            
        validateString surname

    interface IPerson with
        member this.Forename = forename
        member this.Surname = surname
        member this.Fullname = sprintf "%s %s" forename surname

// in F# to access interface properties, methods, etc need to cast the instance to the interface
let person2 = PersonFromInterface("Umair", "But")
(person2 :> IPerson).Fullname |> ignore
