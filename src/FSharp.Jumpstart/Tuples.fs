#if INTERACTIVE
#else
module Tuples
#endif

let RandomPosition() =
    let r = new System.Random()
    r.NextDouble(), r.NextDouble()

let position = RandomPosition()
let x, y = position

// Tupling and function arguments:

// let Area (length, height) = 
//    length * height

// this is referred to as "tupling the function arguments", normally don't do this, but any external library including .NET itself
// requiring multiple arguments will appear from an F# perspective as having tupled arguments
// e.g. let files = System.IO.Directory.EnumerateFiles(@"c:\windows", "*.exe")

let Area length height = 
    length * height

// The un-tupled version can use partial function application
let AreaLength5 = Area 5
