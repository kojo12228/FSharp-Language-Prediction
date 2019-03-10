// Learn more about F# at http://fsharp.org

open System

open Microsoft.ML

open LanguageParser
open Microsoft.ML
open Microsoft.ML.Data

let trainModel () =
    let mlContext = new MLContext()
    let data = mlContext.Data.LoadFromEnumerable(userLanguageRatings)
    //data.Schema |> printfn "%A"
    let dataProcessingPipeline =
        EstimatorChain()
            .Append(mlContext.Transforms.Conversion.MapValueToKey("userIDEncoded", inputColumnName = "userID"))
            .Append(mlContext.Transforms.Conversion.MapValueToKey("languageIDEncoded", inputColumnName = "languageID") :> IEstimator<Transforms.ValueToKeyMappingTransformer>)
    ()
[<EntryPoint>]
let main argv =
    //printfn "Hello World from F#!"
    trainModel()
    idLanguages |> printfn "%A"
    0 // return an integer exit code
