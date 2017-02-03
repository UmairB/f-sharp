module SequencesDemo

open System
open System.IO

let sequence = {0..99}
// '-> i' shorthand for 'yield i'
let sequence2 = seq {
    for i in 0..99 -> i
}
let sequence3 = Seq.init 100 (fun i -> i)
let sequenceInf = Seq.initInfinite (fun i -> i)

// Seq.unfold demo
let isWorkingDay(day : DateTime) =
    day.DayOfWeek <> DayOfWeek.Saturday && day.DayOfWeek <> DayOfWeek.Sunday

let daysFollowing(start : DateTime) =
    Seq.initInfinite (fun i -> start.AddDays(float i))

let workingDaysFollowing start =
    start
    |> daysFollowing
    |> Seq.filter isWorkingDay
    
let nextWorkingDayAfter interval start =
    start
    |> workingDaysFollowing
    |> Seq.item interval

let actionDays startDate endDate interval =
    Seq.unfold(fun date ->
        if date > endDate then None
        else
            let next = date |> nextWorkingDayAfter interval
            let dateString = date.ToString("dd-MMM-yy dddd")
            Some(dateString, next)
    ) startDate

// recursion
let rec safeFileHierarchy startDir = 
    let tryEnumerateFiles dir =
        try
            Directory.EnumerateFiles(dir)
        with _ -> Seq.empty
    let tryEnumerateDirs dir =
        try
            Directory.EnumerateDirectories(dir)
        with _ -> Seq.empty
        
    seq {
        yield! tryEnumerateFiles startDir
        for dir in tryEnumerateDirs startDir do
            yield! (safeFileHierarchy dir)
    }

// cache results
let files = "c:/windows" |> safeFileHierarchy |> Seq.cache
let lastFile = files |> Seq.last
let oneHundredthFile = files |> Seq.item 100

// grouping
let lengthClass length =
    if length < 1024L then "Small"
    else if length < 1024L * 1024L then "Medium"
    else "Large"

let fileSizeGroups dirName = 
    dirName
    |> Directory.EnumerateFiles
    |> Seq.map(fun path ->
        let fileInfo = FileInfo path
        (path, fileInfo.Length))
    |> Seq.groupBy(fun (path, length) -> lengthClass length)

// distinct
let extensions dirName =
    dirName
    |> Directory.EnumerateFiles
    |> Seq.map(fun path -> Path.GetExtension(path))
    |> Seq.distinct

// pairwise
let routeHeights = [| 552; 398; 402; 399; 481; 512; 392; 350 |]
let totalClimb heights =
    heights
    |> Seq.pairwise // group the heights into pairs
    |> Seq.filter(fun (a, b) -> b > a) // filter to those pairs where the second height is greater
    |> Seq.map(fun (a, b) -> b - a) // the actual climbing height
    |> Seq.sum // total climbing height
// windowed - return all the peak heights
let peakHeights heights =
    heights
    |> Seq.choose(fun triplet ->
        match triplet with
        | [|a; b; c|] when b > a && b > c -> Some b
        | _ -> None)
