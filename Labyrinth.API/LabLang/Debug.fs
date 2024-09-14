// Debug.fs
namespace LabLang

open FParsec

module Debug =
    let debugOutput (label: string) (parser: Parser<'a, 'u>): Parser<'a, 'u> =
        fun stream ->
            printfn "Attempting %s on input: %s" label (stream.ToString())
            let result = parser stream
            match result.Status with
            | ReplyStatus.Ok -> printfn "Success in %s: %A" label result.Result
            | ReplyStatus.Error -> printfn "Error in %s: %s" label (result.Error.ToString())
            result
