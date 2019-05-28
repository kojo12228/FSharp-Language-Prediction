module LanguageParser

open FSharp.Data
open MathNet.Numerics.LinearAlgebra

type Survey = CsvProvider<"survey_results_public.csv">

type LanguageRating =
    { userID: int
      languageID: int
      Label: int }

let users = 10000

let survey = new Survey()

let idLanguages =
    let languages =
        seq { for row in survey.Rows do
              yield! row.LanguageDesireNextYear.Split(";") }
        |> Set.ofSeq
        |> Set.toList
        |> List.sort
        |> List.mapi (fun i lang -> (i, lang))
    languages

let userIDMap =
    seq { for row in survey.Rows do
            if row.Respondent < users then
                if row.LanguageDesireNextYear <> "NA" then
                    yield row.Respondent }
    |> Seq.mapi (fun i x -> (x, i))
    |> Map.ofSeq

let userLanguageRatings =
    seq { for row in survey.Rows do
          if row.Respondent < users then
              if row.LanguageDesireNextYear <> "NA" then
                  let userLanguages = row.LanguageDesireNextYear.Split(";")
                  for id, lang in idLanguages do
                      yield 
                          if Array.contains lang userLanguages
                          then {userID = row.Respondent; languageID = id; Label = 1}
                          else {userID = row.Respondent; languageID = id; Label = 0} }

let userLanguageMap() =
    userLanguageRatings
    |> Seq.groupBy (fun x -> x.languageID)
    |> Seq.map (fun (lID, ratings) ->
        lID, (ratings |> Seq.filter (fun x -> x.Label = 1) |> Seq.map (fun x -> x.userID)))
    |> Map.ofSeq

let ratingMatrix =
    let groupedULR =
        userLanguageRatings
        |> Seq.groupBy (fun x -> x.userID)
    let ratingsVector ratings =
        [ for x in ratings -> x.Label |> float ]
    [ for _, userRatings in groupedULR -> ratingsVector userRatings ]
    |> matrix

let svd =
    ratingMatrix
    |> Matrix.svd

let approxRatingMatrix =
    let sigmaMatrix =
        svd.W
        |> Matrix.mapi (fun i j x -> if i = j && i < 20 then x else 0.)
    svd.U * sigmaMatrix * svd.VT

let predictRating userID languageID =
    approxRatingMatrix.[userIDMap.[userID], languageID]

let predictUserRatings userID =
    approxRatingMatrix.Row(userIDMap.[userID])