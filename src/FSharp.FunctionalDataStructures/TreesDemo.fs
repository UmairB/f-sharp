module TreesDemo

// OO approach to tree-like structures
type FoodStuff(name : string, foodStuffs : FoodStuff list) =
    member this.Name = name
    member this.FoodStuffs = foodStuffs

let cake = 
    FoodStuff("cake",
        [
            FoodStuff("flour", [])
            FoodStuff("egg", [])
            FoodStuff("mixed fruit",
                [
                    FoodStuff("raisin", [])
                    FoodStuff("saltana", [])
                ])
            FoodStuff("mixed nuts",
                [
                    FoodStuff("almond", [])
                    FoodStuff("peanut", [])
                ])
        ])

let rec allIngredients (foodStuff : FoodStuff) =
    seq {
        if foodStuff.FoodStuffs.Length = 0 then
            yield foodStuff.Name
        else
            for i in foodStuff.FoodStuffs do
                yield! allIngredients i
    }

let rec hasIngredient (foodStuff : FoodStuff) ingredient =
    foodStuff.Name = ingredient || foodStuff.FoodStuffs |> List.exists(fun i -> hasIngredient i ingredient)

// Discriminated union approach
type Description = 
    {
        Name : string
        PartNumber : string
        Cost : decimal option
    }

type Part = 
    | Item of Description
    | Repeat of Part * int
    | Compound of Description * Part list

let pad = Item { Name = "Brake Pad"; PartNumber = "BP1234"; Cost = Some 15.90M }
let calliperBody = Item { Name = "Calliper Body"; PartNumber = "CB4568"; Cost = Some 10.00M }
let brakePiston = Item { Name = "Breake Piston"; PartNumber = "PS3142"; Cost = Some 5.10M }
let disc = Item { Name = "Disc"; PartNumber = "D23432"; Cost = Some 18.45M }
let clip = Item { Name = "Clip"; PartNumber = "C22345"; Cost = Some 2.0M }
let pin = Item { Name = "Pin"; PartNumber = "P53585"; Cost = Some 1.35M }
let calliper = 
    Compound (
        { Name = "Calliper"; PartNumber = "C25838"; Cost = None },
        [calliperBody; Repeat(brakePiston, 2)]
    )
let brake = Compound (
    { Name = "Brake"; PartNumber = "B23571"; Cost = None },
    [disc; calliper; Repeat(pin, 2); Repeat(pad, 2); clip]
)

let flatten (part: Part) =
    let rec flattenRec p =
        seq {
            match p with
            | Item d ->
                yield d
            | Repeat(rp, count) ->
                for _ in 0..count-1 do
                    yield! flattenRec rp
            | Compound(d, children) ->
                yield d
                for child in children do
                    yield! flattenRec child
        }

    flattenRec part

let totalCost part =
    part
    |> flatten
    |> Seq.choose(fun p -> p.Cost)
    |> Seq.sum
    // part 
    // |> flatten 
    // |> Seq.sumBy(fun p -> 
    //    match d.Cost with 
    //    | Some c -> c
    //    | None -> 0.0M
    // )
