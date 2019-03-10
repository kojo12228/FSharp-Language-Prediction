module LanguageParser

open FSharp.Data

//let fileLocation = @"survey_results_public.csv"

type Survey = CsvProvider<"survey_results_public.csv">

//type LanguageRating(userID, languageID, label) =
//    new() = LanguageRating(0,0,0)
//    member x.userID = userID
//    member x.languageID = languageID
//    member x.Label = label

type LanguageRating =
    { userID: int
      languageID: int
      Label: int }


let survey = new Survey()

let idLanguages =
    let languages =
        seq { for row in survey.Rows do
              yield! row.LanguageWorkedWith.Split(";") }
        |> Set.ofSeq
        |> Set.toList
        |> List.sort
        |> List.mapi (fun i lang -> (i, lang))
    languages

let userLanguageRatings =
    seq { for row in survey.Rows do
          if row.LanguageWorkedWith <> "NA" then
              let userLanguages = row.LanguageWorkedWith.Split(";")
              for id, lang in idLanguages do
                  yield 
                      if Array.contains lang userLanguages
                      then {userID = row.Respondent; languageID = id; Label = 1}
                      else {userID = row.Respondent; languageID = id; Label = 0} }
