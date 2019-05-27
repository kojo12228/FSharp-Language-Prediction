// Learn more about F# at http://fsharp.org

open System

open Microsoft.ML

open LanguageParser
open MathNet.Numerics.LinearAlgebra

[<EntryPoint>]
let main argv =
    //printfn "Hello World from F#!"
    //trainModel()
    //idLanguages |> printfn "%A"
    idLanguages |> printfn "%A"

    //let x = svd
    //x.S.Count |> printfn "%A"

    //predictRating 8 3 |> printfn "C# Prediction: %f"
    //for i in predictUserRatings 8 do
    //    printfn "%f" i

    decomposedRatingMatrix.RowCount
    |> printfn "%d"

    let languageMap = userLanguageMap()
    let languageAverage =
        [ for userID in languageMap.[16] ->
          predictUserRatings userID |> Vector.toList ]
        |> matrix
        |> Matrix.toColSeq
        |> Seq.map (Seq.average)
        |> Seq.zip idLanguages
    for x in languageAverage do
        printfn "%A" x


    0 // return an integer exit code