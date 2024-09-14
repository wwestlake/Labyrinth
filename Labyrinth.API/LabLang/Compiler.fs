// Compiler.fs
namespace LabLang

open LabLang.AST
open LabLang.Parsers
open FluentResults
open Labyrinth.Common
open System.Collections.Generic
open FParsec

module Compiler =

    // Convert F# Direction to C# string
    let directionToString direction =
        match direction with
        | North -> "north"
        | South -> "south"
        | East  -> "east"
        | West  -> "west"
        | Up    -> "up"
        | Down  -> "down"

    // Convert F# AST to C# AST
    let rec toCsAst (command: Command): CommandAst =
        match command with
        | Move direction ->
            MoveCommandAst(Direction = directionToString direction) :> CommandAst
        | Look ->
            LookCommandAst() :> CommandAst
        | LookAt target ->
            LookAtCommandAst(Target = target) :> CommandAst
        | PickUp item ->
            PickUpCommandAst(Item = item) :> CommandAst
        | Drop item ->
            DropCommandAst(Item = item) :> CommandAst
        | Open target ->
            OpenCommandAst(Target = target) :> CommandAst
        | Close target ->
            CloseCommandAst(Target = target) :> CommandAst
        | Say message ->
            SayCommandAst(Message = message) :> CommandAst
        | Whisper (target, message) ->
            WhisperCommandAst(Target = target, Message = message) :> CommandAst
        | Attack target ->
            AttackCommandAst(Target = target) :> CommandAst
        | Cast (spell, target) ->
            CastCommandAst(Spell = spell, Target = target) :> CommandAst
        | Expr expr ->
            match expr with
            | Literal value -> LiteralCommandAst(value) :> CommandAst
            | Variable name -> VariableCommandAst(name) :> CommandAst
            | BinaryOp(op, left, right) ->
                BinaryOpCommandAst(op, toCsAst (Expr left), toCsAst (Expr right)) :> CommandAst
            | UnaryOp(op, expr) ->
                UnaryOpCommandAst(op, toCsAst (Expr expr)) :> CommandAst
            | FunctionCall(name, args) ->
                FunctionCallCommandAst(name, new List<CommandAst>(args |> List.map (fun arg -> toCsAst (Expr arg)))) :> CommandAst
            | Lambda(params, body) ->
                LambdaCommandAst(new List<string>(params), toCsAst (Expr body)) :> CommandAst
            | Conditional(cond, thenExpr, elseExpr) ->
                ConditionalCommandAst(
                    toCsAst (Expr cond),
                    toCsAst (Expr thenExpr),
                    elseExpr |> Option.map (fun e -> toCsAst (Expr e)) |> Option.defaultValue null
                ) :> CommandAst
            | Loop(loopType, cond, body) ->
                LoopCommandAst(
                    loopType.toString(),
                    toCsAst (Expr cond),
                    toCsAst (Expr body)
                ) :> CommandAst
            | Let(name, value, body) ->
                LetCommandAst(name, toCsAst (Expr value), toCsAst (Expr body)) :> CommandAst
            | List exprs ->
                ListCommandAst(new List<CommandAst>(exprs |> List.map (fun e -> toCsAst (Expr e)))) :> CommandAst
            | Map(fn, lst) ->
                MapCommandAst(toCsAst (Expr fn), toCsAst (Expr lst)) :> CommandAst
            | Fold(fn, acc, lst) ->
                FoldCommandAst(toCsAst (Expr fn), toCsAst (Expr acc), toCsAst (Expr lst)) :> CommandAst
        | DefineFunction(name, params, body) ->
            DefineFunctionCommandAst(name, new List<string>(params), toCsAst (Expr body)) :> CommandAst        
        | TryCatch(tryExpr, catchExpr) ->
            TryCatchCommandAst(toCsAst (Expr tryExpr), toCsAst (Expr catchExpr)) :> CommandAst

    // Function that the API can call to parse the command and return a C# AST or error
    let compileCommand (input: string): FluentResults.Result<CommandAst> =
        printfn "Starting to parse input: %s" input
        let parseResult = run (parseCommand .>> eof) input
        match parseResult with
        | Success(fsharpAst, _, _) ->
            // Convert the F# AST to C# AST
            let csAst = toCsAst fsharpAst
            printfn "Parsing successful! Resulting AST: %A" csAst
            FluentResults.Result.Ok(csAst)
        | Failure(errorMsg, _, _) ->
            // Parsing failed, return the error message
            printfn "Parsing failed with error: %s" errorMsg
            FluentResults.Result.Fail(errorMsg)

