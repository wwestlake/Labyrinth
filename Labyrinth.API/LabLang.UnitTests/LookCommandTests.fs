module LabLang.UnitTests.LookCommandTests

open Xunit
open FsUnit.Xunit
open LabLang
open Labyrinth.Common
open FluentResults

[<Fact>]
let ``Test parsing of Look command`` () =
    // Arrange
    let input = "look"

    // Act
    let result = LabLang.Compiler.compileCommand input

    // Assert
    match result with
    | r when r.IsSuccess ->
        match r.Value with
        | :? LookCommandAst -> () // Test passes if LookCommandAst is returned
        | _ -> failwith "Expected a LookCommandAst"
    | r when r.IsFailed ->
        failwithf "Parsing failed with error: %s" (r.Errors |> Seq.map (fun e -> e.Message) |> String.concat ", ")
    | _ -> failwith "Unexpected result type"
