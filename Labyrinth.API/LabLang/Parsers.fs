// Parser.fs
namespace LabLang

open FParsec
open LabLang.AST
open Labyrinth.Common

module Parsers =

    // Define whitespace parsers
    let ws: Parser<unit, unit> = spaces
    let ws1: Parser<unit, unit> = spaces1 // matches at least one whitespace character

    // Forward declarations using createParserForwardedToRef for recursive parsing
    let parseExpression, parseExpressionRef = createParserForwardedToRef<Expression, unit>()
    let parseCommand, parseCommandRef = createParserForwardedToRef<Command, unit>()

    // Helper function: Checks if a character can be part of an identifier
    let isIdentifierChar c = isLetter c || isDigit c || c = '_'

    let parseLiteral: Parser<Expression, unit> =
        choice [
            pfloat |>> fun n -> Literal (box n)
            pint32 |>> fun n -> Literal (box n)
            many1SatisfyL (fun c -> isLetter c || isDigit c || c = '_' || c = ' ') "string" |>> fun s -> Literal (box s)
        ] <?> "parseLiteral"

    let parseVariable: Parser<Expression, unit> =
        many1Satisfy isIdentifierChar |>> Variable <?> "parseVariable"


    let parseFunctionCall: Parser<Expression, unit> =
        parseVariable .>> ws .>>. between (pchar '(') (pchar ')') (sepBy (parseExpression .>> ws) (pchar ',' >>. ws)) |>>
        (fun (name, args) ->
            match name with
            | Variable variableName ->
                printfn "Parsed function: %s with arguments: %A" variableName args
                FunctionCall(variableName, args)
            | _ -> failwith "Expected a variable for the function name"
        ) <?> "parseFunctionCall"

    let parseTerm: Parser<Expression, unit> =
        choice [
            parseVariable
            parseLiteral
            parseFunctionCall
            between (pchar '(') (pchar ')') parseExpression
        ] <?> "parseTerm"

    let parseBinaryOp: Parser<Expression, unit> =
        let operators = [
            // Logical operators (lowest precedence)
            ["&&"; "||"];
        
            // Relational operators (next level of precedence)
            ["=="; "!="; "<"; "<="; ">"; ">="];
        
            // Addition and subtraction (higher precedence)
            ["+"; "-"];
        
            // Multiplication and division (highest precedence)
            ["*"; "/"]
        ]

        // Helper function to create a binary operator parser for a given list of operators
        let createOpParser (opList: string list): Parser<(Expression -> Expression -> Expression), unit> =
            choice (opList |> List.map (fun op -> pstring op >>% (fun left right -> BinaryOp(op, left, right))))

        // Recursively build the parser by chaining lower-precedence operators with higher-precedence ones
        let rec buildBinaryParser (prevParser: Parser<Expression, unit>) (opGroups: string list list): Parser<Expression, unit> =
            match opGroups with
            | [] -> prevParser
            | ops :: rest -> chainl1 prevParser (ws >>. createOpParser ops .>> ws) |> fun nextParser -> buildBinaryParser nextParser rest

        // Start with parsing basic terms and build the parser by chaining operator groups
        buildBinaryParser parseTerm operators



    let parseConditional: Parser<Expression, unit> =
        pstring "if" >>. ws1 >>. parseExpression .>> ws1
        .>>. (pstring "then" >>. ws1 >>. parseExpression) .>> ws1
        .>>. (pstring "else" >>. ws1 >>. parseExpression)
        |>> fun ((condition, thenExpr), elseExpr) -> Conditional(condition, thenExpr, Some elseExpr)
        <?> "parseConditional"


    let parseExpressionImpl: Parser<Expression, unit> =
        choice [
            parseConditional
            parseBinaryOp
            parseFunctionCall     // Handle function calls like fact(n - 1)
            parseTerm
        ] <?> "parseExpression"

    do parseExpressionRef := parseExpressionImpl

    let parseCommandImpl: Parser<Command, unit> =
        let parseDirection: Parser<Direction, unit> =
            choice [
                stringReturn "north" North
                stringReturn "south" South
                stringReturn "east" East
                stringReturn "west" West
                stringReturn "up" Up
                stringReturn "down" Down
            ] <?> "parseDirection"

        let parseMove: Parser<Command, unit> =
            pstring "go" >>. ws >>. parseDirection |>> Move <?> "parseMove"

        let parseLook: Parser<Command, unit> =
            pstring "look" >>% Look <?> "parseLook"

        let parseLookAt: Parser<Command, unit> =
            pstring "look at" >>. ws >>. many1Satisfy isLetter |>> LookAt <?> "parseLookAt"

        let parsePickUp: Parser<Command, unit> =
            pstring "pick up" >>. ws >>. many1Satisfy isLetter |>> PickUp <?> "parsePickUp"

        let parseFunctionDefinition: Parser<Command, unit> =
            pstring "def" >>. ws1                                // Match 'def' keyword
            >>. many1Satisfy isIdentifierChar .>> ws             // Parse function name
            .>>. between (pchar '(') (pchar ')')                 // Parameters in parentheses
                (sepBy (many1Satisfy isIdentifierChar .>> ws) (pchar ',' >>. ws)) // Comma-separated parameters
            .>> ws .>> pstring "->" .>> ws                      // Match '->' and consume surrounding whitespace
            .>>. parseExpression                                // Parse the function body expression
            |>> fun ((name, params), body) -> DefineFunction(name, params, body)  // Return DefineFunction AST node
            <?> "parseFunctionDefinition"


        choice [
            parseFunctionDefinition
            parseMove
            parseLook
            parseLookAt
            parsePickUp
            (parseExpression |>> Expr)
        ] <?> "parseCommand"

    do parseCommandRef := parseCommandImpl
