namespace LabLang

open FParsec
open Labyrinth.Common
open FluentResults

module LabLang =

    // Define the direction type
    type Direction =
        | North
        | South
        | East
        | West
        | Up
        | Down

    // Define the F# Command AST
    type Command =
        | Move of Direction
        | Look
        | LookAt of string
        | PickUp of string
        | Drop of string
        | Open of string
        | Close of string
        | Say of string
        | Whisper of string * string
        | Attack of string
        | Cast of string * string

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
    let toCsAst (command: Command): CommandAst =
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

    // Parser functions for F# AST (for example purposes)
    let parseDirection: Parser<Direction, unit> =
        choice [
            stringReturn "north" North
            stringReturn "south" South
            stringReturn "east" East
            stringReturn "west" West
            stringReturn "up" Up
            stringReturn "down" Down
        ]

    let parseMove: Parser<Command, unit> =
        pstring "go" >>. spaces1 >>. parseDirection |>> Move

    let parseLook: Parser<Command, unit> =
        pstring "look" >>% Look

    let parseLookAt: Parser<Command, unit> =
        pstring "look at" >>. spaces1 >>. many1Satisfy isLetter |>> LookAt

    let parsePickUp: Parser<Command, unit> =
        pstring "pick up" >>. spaces1 >>. many1Satisfy isLetter |>> PickUp

    // Combine the individual parsers into a single parser
    let parseInput: Parser<Command, unit> =
        choice [
            parseMove
            parseLook
            parseLookAt
            parsePickUp
        ]

     // Function that the API can call to parse the command and return a C# AST or error
    let compileCommand (input: string): FluentResults.Result<CommandAst> =
        match run (parseInput .>> eof) input with
        | Success(fsharpAst, _, _) ->
            // Convert the F# AST to C# AST
            let csAst = toCsAst fsharpAst
            FluentResults.Result.Ok(csAst)
    
        | Failure(errorMsg, _, _) ->
            // Parsing failed, return the error message
            FluentResults.Result.Fail(errorMsg)