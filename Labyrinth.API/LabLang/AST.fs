// AST.fs
namespace LabLang

open System.Collections.Generic // Needed for List type conversion

module AST =

    // Define types for the enhanced command language
    type DataType =
        | IntType
        | FloatType
        | StringType
        | BoolType
        | ListType of DataType
        | FunctionType of DataType list * DataType

    type Direction =
        | North
        | South
        | East
        | West
        | Up
        | Down

    type Expression =
        | Literal of obj
        | Variable of string
        | BinaryOp of string * Expression * Expression
        | UnaryOp of string * Expression
        | FunctionCall of string * Expression list
        | Lambda of string list * Expression
        | Conditional of Expression * Expression * Expression option
        | Loop of LoopType * Expression * Expression
        | Let of string * Expression * Expression
        | List of Expression list
        | Map of Expression * Expression
        | Fold of Expression * Expression * Expression

    and LoopType =
        | While
        | For
        member this.toString() =
            match this with
            | While -> "while"
            | For -> "for"

    type Command =
        | Expr of Expression
        | DefineFunction of string * string list * Expression
        | TryCatch of Expression * Expression
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
