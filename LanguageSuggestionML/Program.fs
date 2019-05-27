// Learn more about F# at http://fsharp.org

open System

open LanguageParser
open MathNet.Numerics.LinearAlgebra

let averagesGivenLanguage languageID =
    let languageMap = userLanguageMap()
    [ for userID in languageMap.[languageID] ->
        predictUserRatings userID |> Vector.toList ]
    |> matrix
    |> Matrix.toColSeq
    |> Seq.map (Seq.average)
    |> Seq.zip idLanguages
    |> Seq.map (fun ((_, lang), pred) -> lang, pred)

[<EntryPoint>]
let main argv =
    idLanguages |> printfn "%A"

    decomposedRatingMatrix.RowCount
    |> printfn "%d"

    let mutable csv =
        "Pred Lang," +
        String.Join(',',[ for id, lang in idLanguages -> lang]) +
        "\n"
    let idLangMap = Map.ofList idLanguages
    for langID in 0 .. 38 do
        if langID <> 23 then
            let averages =
                averagesGivenLanguage langID
                |> Seq.map snd
            let rowString =
                idLangMap.[langID] + "," +
                 String.Join(',',averages) +
                 "\n"
            csv <- csv + rowString
    printfn "%s" csv
    System.IO.File.WriteAllLines("desired_results.csv", [csv])

    0 // return an integer exit code