module LabLang.UnitTests.DefineFunctionCommandTests

open Xunit
open FsUnit.Xunit
open LabLang
open Labyrinth.Common
open FluentResults

// Test DefineFunction command with the new syntax using explicit Assert checks
[<Fact>]
let ``Test parsing of DefineFunction command`` () =
    // Arrange
    let input = "def add (x, y) -> x + y"  // Updated to reflect new syntax

    // Act
    let result: Result<CommandAst> = LabLang.Compiler.compileCommand input

    // Assert
    if result.IsSuccess then
        let command = result.Value
        match command with
        | :? DefineFunctionCommandAst as defineAst ->
            Assert.Equal("add", defineAst.Name)
            Assert.Equal(["x"; "y"], defineAst.Params)  // Parameters should now be parsed as "x" and "y"

            match defineAst.Body with
            | :? BinaryOpCommandAst as binaryOpAst ->
                Assert.Equal("+", binaryOpAst.Op)

                match binaryOpAst.Left, binaryOpAst.Right with
                | (:? VariableCommandAst as left), (:? VariableCommandAst as right) ->
                    Assert.Equal("x", left.Name)
                    Assert.Equal("y", right.Name)
                | _ -> Assert.True(false, "Expected BinaryOpCommandAst to have VariableCommandAst for both Left and Right operands.")
            | _ -> Assert.True(false, "Expected the body of DefineFunction to be a BinaryOpCommandAst.")
        | _ -> Assert.True(false, "Expected a DefineFunctionCommandAst as the parsed result.")
    else
        let errorMessage = result.Errors |> Seq.map (fun e -> e.Message) |> String.concat ", "
        Assert.True(false, sprintf "Parsing failed with error: %s" errorMessage)
