module MapsDemo

// Maps are immutable dictionaries in F#
let moons = Map([("Mercury", 0); ("Venus", 0); ("Earth", 1); ("Mars", 2)])
let earthMoons = moons.["Earth"]

let moons' = moons.Add("Jupiter", 63)

let elements = 
    [("Hydrogen", 1); ("Helium", 2); ("Lithium", 3); ("Beryllium", 4)]
    |> Map.ofList

// NOTES
//  .NET dictionaries are faster than F# maps and dict, F# dict are faster than maps
//      - for creation and retrieval
