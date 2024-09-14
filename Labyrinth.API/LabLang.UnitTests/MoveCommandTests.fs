module LabLang.UnitTests.MoveCommandTests

open Xunit
open FsUnit.Xunit
open LabLang
open Labyrinth.Common
open FluentResults


[<Theory>]
[<InlineData("go north", "north")>]
[<InlineData("go south", "south")>]
[<InlineData("go east", "east")>]
[<InlineData("go west", "west")>]
[<InlineData("go up", "up")>]
[<InlineData("go down", "down")>]
let ``Test parsing of Move command with various directions`` (input: string, expectedDirection: string) =
    // Act
    let result = LabLang.Compiler.compileCommand input

    // Assert
    match result with
    | :? Result<CommandAst> as commandResult when commandResult.IsSuccess ->
        let moveAst = commandResult.Value :?> MoveCommandAst
        moveAst.Direction |> should equal expectedDirection
    | :? Result<CommandAst> as commandResult when commandResult.IsFailed ->
        failwithf "Parsing failed with error: %s" (commandResult.Errors |> Seq.map (fun e -> e.Message) |> String.concat ", ")
    | _ -> failwith "Unexpected result type"

[<Fact>]
let ``Test parsing of Move command with invalid direction`` () =
    // Arrange
    let input = "go sideways"

    // Act
    let result = LabLang.Compiler.compileCommand input

    // Assert
    match result with
    | :? Result<CommandAst> as commandResult when commandResult.IsFailed ->
        commandResult.Errors |> should not' (be Empty)
    | :? Result<CommandAst> as commandResult when commandResult.IsSuccess ->
        failwith "Expected parsing to fail, but it succeeded."
    | _ -> failwith "Unexpected result type"
