namespace Labyrinth.Languages.Templates

open FParsec
open System
open FSharp.Core

// Define types for placeholders, conditions, and custom functions
type Placeholder = string
type Condition = string * string * string  // (variable, operator, value)
type CustomFunction = string * string list // (function name, arguments)

// Define the AST (Abstract Syntax Tree) for the template
type TemplateElement =
    | Text of string
    | Placeholder of Placeholder
    | Conditional of Condition * TemplateElement list * TemplateElement list option
    | Loop of string * TemplateElement list
    | Function of CustomFunction
    | Error of string

module TemplateParser =

    // Placeholder parser: {{placeholder}}
    let placeholderParser: Parser<TemplateElement, unit> =
        between (pstring "{{") (pstring "}}") (many1Satisfy (fun c -> c <> '}'))
        |>> (fun placeholder -> Placeholder placeholder)

    // Conditional parser: {{#if condition}} ... {{else}} ... {{/if}}
    let conditionParser: Parser<TemplateElement, unit> =
        let conditionExpr = between (pstring "{{#if ") (pstring "}}") (many1Satisfy (fun c -> c <> '}'))
        let elseExpr = pstring "{{else}}" >>. (manyTill anyChar (pstring "{{/if}}")) |>> (fun elseText -> [Text (new string(Array.ofList elseText))])
        let trueExpr = (manyTill anyChar (lookAhead (pstring "{{else}}") <|> pstring "{{/if}}")) |>> (fun trueText -> [Text (new string(Array.ofList trueText))])
        pipe3 conditionExpr trueExpr (opt elseExpr) (fun cond trueBranch elseBranch -> Conditional((cond, "==", "true"), trueBranch, elseBranch))

    // Loop parser: {{#each items}} ... {{/each}}
    let loopParser: Parser<TemplateElement, unit> =
        let loopHeader = between (pstring "{{#each ") (pstring "}}") (many1Satisfy (fun c -> c <> '}'))
        let loopBody = manyTill anyChar (pstring "{{/each}}") |>> (fun body -> [Text (new string(Array.ofList body))])
        pipe2 loopHeader loopBody (fun variable body -> Loop(variable, body))

    // Custom function parser: {{customFunction arg1 arg2}}
    let functionParser: Parser<TemplateElement, unit> =
        let functionHeader = between (pstring "{{") (pstring "}}") (sepBy (many1Satisfy (fun c -> c <> ' ')) spaces1)
        functionHeader |>> (fun parts -> Function(parts.[0], List.tail parts))

    // Text parser: Any text not within {{...}}
    let textParser: Parser<TemplateElement, unit> =
        many1Satisfy (fun c -> c <> '{') |>> Text

    // Main parser combining all elements
    let templateParser: Parser<TemplateElement list, unit> =
        many (choice [placeholderParser; conditionParser; loopParser; functionParser; textParser])

    // Parse a template string into an AST
    let parseTemplate (template: string) : Result<TemplateElement list, string> =
        match run templateParser template with
        | Success(result, _, _) -> Ok result
        | ParserResult.Failure(errorMsg, _, _) -> Result.Error errorMsg

module TemplateRenderer =
    open TemplateParser

    // Helper function to replace placeholders with values from context
    let replacePlaceholders (context: Map<string, string>) (placeholder: string) : string =
        context.TryFind placeholder |> Option.defaultValue ""

    // Evaluate conditions based on the context
    let evaluateCondition (context: Map<string, string>) (condition: Condition) : bool =
        let (variable, op, value) = condition
        match context.TryFind variable with
        | Some(v) when op = "==" -> v = value
        | Some(v) when op = "!=" -> v <> value
        | _ -> false

    // Render the template AST to a final string output
    let rec renderTemplate (context: Map<string, string>) (elements: TemplateElement list) : string =
        elements
        |> List.map (fun element ->
            match element with
            | Text(text) -> text
            | Placeholder(placeholder) -> replacePlaceholders context placeholder
            | Conditional(condition, trueBranch, falseBranch) ->
                if evaluateCondition context condition then
                    renderTemplate context trueBranch
                else
                    match falseBranch with
                    | Some(branch) -> renderTemplate context branch
                    | None -> ""
            | Loop(variable, body) ->
                match context.TryFind variable with
                | Some(items) ->
                    // Assume items is a comma-separated list for simplicity
                    let itemsList = items.Split(',')
                    itemsList |> Array.map (fun item -> renderTemplate (context.Add(variable, item)) body) |> String.concat ""
                | None -> ""
            | Function(functionName, args) -> 
                // Placeholder for custom function logic
                sprintf "%s(%s)" functionName (String.concat ", " args)
            | Error(error) -> error)
        |> String.concat ""

    // Entry function to parse and render a template
    let render (template: string) (context: Map<string, string>) : Result<string, string> =
        match parseTemplate template with
        | Ok ast -> Ok (renderTemplate context ast)  // Corrected to wrap the result in Ok
        | Result.Error err -> Result.Error err
