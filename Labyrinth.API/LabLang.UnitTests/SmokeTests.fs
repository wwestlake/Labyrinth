module LabLang.UnitTests.SmokeTests

open Xunit
open FsUnit.Xunit
open LabLang
open FluentResults
open Labyrinth.Common

// Define a Theory test with various valid commands
[<Theory>]
[<InlineData("def add (x, y) -> x + y")>]     // Simple function definition with addition
[<InlineData("def mul (a, b) -> a * b")>]     // Function with multiplication
[<InlineData("def noOp () -> 1")>]            // No-parameter function returning a literal
[<InlineData("go north")>]                    // Movement command (north)
[<InlineData("look")>]                        // Look command
[<InlineData("pick up key")>]                 // Pick up an item
[<InlineData("def id (x) -> x")>]             // Identity function (simple return)
[<InlineData("say 'Hello'")>]                 // Say command (dialogue)
[<InlineData("look at door")>]                // Look at a specific object
[<InlineData("def combine (x, y) -> x + (y * 2)")>]  // Combination of binary operations
[<InlineData("def cond (x) -> if x > 0 then x else 0")>]  // Simple conditional function
[<InlineData("go east")>]                     // Another movement command
[<InlineData("def fold (f, acc, lst) -> fold(f, acc, lst)")>] // Recursive function example
[<InlineData("def fact (n) -> if n <= 1 then 1 else n * fact(n - 1)")>] // Factorial function
let ``Smoke test for valid commands`` (input: string) =
    // Act
    let result: Result<CommandAst> = LabLang.Compiler.compileCommand input

    // Assert: Ensure no error is returned from the compiler
    if result.IsFailed then
        let errorMessage = result.Errors |> Seq.map (fun e -> e.Message) |> String.concat ", "
        Assert.True(false, sprintf "Compilation failed for: %s\nError: %s" input errorMessage)


