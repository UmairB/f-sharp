module ListsDemo

// lists in F# are implemented with a singly linked-list, hence list access is O(n), since the list needs to be traversed 
// from the beginning to retrieve the nth element
let list = [0..99]
let list2 = [for i in 0..99 -> i]

let describeList list = 
    match list with
    | [] -> "Empty list"
    | head::tail ->
        sprintf "List beginning %A, %i more elements" head tail.Length

// pattern matching with lists example
type SimpleStack<'T>() =
    let mutable _stack : List<'T> = []

    member this.Push value =
        _stack <- value :: _stack

    member this.Pop value =
        match _stack with
        | result::remainder ->
            _stack <- remainder
            result
        | [] -> failwith "Empty stack"

    member this.TryPop value =
        match _stack with
        | result::remainder ->
            _stack <- remainder
            result |> Some
        | [] -> None

    // Swap first two elements
    member this.Swap() =
        match _stack with
        | a::b::tail -> _stack <- b::a::tail
        | _ -> failwith "Stack does not have enough elements"

// recursive list traversal
let partitionUntil predicate input =
    let rec loop acc list =
        match list with
        | head::tail when predicate head -> List.rev acc, head::tail // :: adds things to the beginning of the list, hence reversiing the order, so need to reverse
        | head::tail -> loop (head::acc) tail
        | [] -> input, []
    loop [] input
