namespace Labyrinth.Common
{
    // Base abstract class for all command types
    public class CommandAst { }

    // Command class for movement actions
    public class MoveCommandAst : CommandAst
    {
        public string Direction { get; set; }
    }

    // Command class for looking around
    public class LookCommandAst : CommandAst { }

    // Command class for looking at a specific target
    public class LookAtCommandAst : CommandAst
    {
        public string Target { get; set; }
    }

    // Command class for picking up an item
    public class PickUpCommandAst : CommandAst
    {
        public string Item { get; set; }
    }

    // Command class for dropping an item
    public class DropCommandAst : CommandAst
    {
        public string Item { get; set; }
    }

    // Command class for opening something
    public class OpenCommandAst : CommandAst
    {
        public string Target { get; set; }
    }

    // Command class for closing something
    public class CloseCommandAst : CommandAst
    {
        public string Target { get; set; }
    }

    // Command class for saying something aloud
    public class SayCommandAst : CommandAst
    {
        public string Message { get; set; }
    }

    // Command class for whispering to someone
    public class WhisperCommandAst : CommandAst
    {
        public string Target { get; set; }
        public string Message { get; set; }
    }

    // Command class for attacking a target
    public class AttackCommandAst : CommandAst
    {
        public string Target { get; set; }
    }

    // Command class for casting a spell on a target
    public class CastCommandAst : CommandAst
    {
        public string Spell { get; set; }
        public string Target { get; set; }
    }

    // Command class for variable usage
    public class VariableCommandAst : CommandAst
    {
        public string Name { get; set; }

        public VariableCommandAst(string name)
        {
            Name = name;
        }
    }

    // Command class for literal values
    public class LiteralCommandAst : CommandAst
    {
        public object Value { get; set; }

        public LiteralCommandAst(object value)
        {
            Value = value;
        }
    }

    // Command class for binary operations (e.g., +, -, *, /)
    public class BinaryOpCommandAst : CommandAst
    {
        public string Op { get; set; }
        public CommandAst Left { get; set; }
        public CommandAst Right { get; set; }

        public BinaryOpCommandAst(string op, CommandAst left, CommandAst right)
        {
            Op = op;
            Left = left;
            Right = right;
        }
    }

    // Command class for unary operations (e.g., -x)
    public class UnaryOpCommandAst : CommandAst
    {
        public string Op { get; set; }
        public CommandAst Expr { get; set; }

        public UnaryOpCommandAst(string op, CommandAst expr)
        {
            Op = op;
            Expr = expr;
        }
    }

    // Command class for function calls
    public class FunctionCallCommandAst : CommandAst
    {
        public string Name { get; set; }
        public List<CommandAst> Args { get; set; }

        public FunctionCallCommandAst(string name, List<CommandAst> args)
        {
            Name = name;
            Args = args;
        }
    }

    // Command class for lambda expressions
    public class LambdaCommandAst : CommandAst
    {
        public List<string> Params { get; set; }
        public CommandAst Body { get; set; }

        public LambdaCommandAst(List<string> parameters, CommandAst body)
        {
            Params = parameters;
            Body = body;
        }
    }

    // Command class for conditional expressions (if-then-else)
    public class ConditionalCommandAst : CommandAst
    {
        public CommandAst Condition { get; set; }
        public CommandAst Then { get; set; }
        public CommandAst? Else { get; set; }

        public ConditionalCommandAst(CommandAst condition, CommandAst thenExpr, CommandAst? elseExpr = null)
        {
            Condition = condition;
            Then = thenExpr;
            Else = elseExpr;
        }
    }

    // Command class for loops (while or for)
    public class LoopCommandAst : CommandAst
    {
        public string LoopType { get; set; }  // "while" or "for"
        public CommandAst Condition { get; set; }
        public CommandAst Body { get; set; }

        public LoopCommandAst(string loopType, CommandAst condition, CommandAst body)
        {
            LoopType = loopType;
            Condition = condition;
            Body = body;
        }
    }

    // Command class for let expressions (variable assignment)
    public class LetCommandAst : CommandAst
    {
        public string Name { get; set; }
        public CommandAst Value { get; set; }
        public CommandAst Body { get; set; }

        public LetCommandAst(string name, CommandAst value, CommandAst body)
        {
            Name = name;
            Value = value;
            Body = body;
        }
    }

    // Command class for list expressions
    public class ListCommandAst : CommandAst
    {
        public List<CommandAst> Elements { get; set; }

        public ListCommandAst(List<CommandAst> elements)
        {
            Elements = elements;
        }
    }

    // Command class for map operations
    public class MapCommandAst : CommandAst
    {
        public CommandAst Function { get; set; }
        public CommandAst List { get; set; }

        public MapCommandAst(CommandAst function, CommandAst list)
        {
            Function = function;
            List = list;
        }
    }

    // Command class for fold operations
    public class FoldCommandAst : CommandAst
    {
        public CommandAst Function { get; set; }
        public CommandAst Accumulator { get; set; }
        public CommandAst List { get; set; }

        public FoldCommandAst(CommandAst function, CommandAst accumulator, CommandAst list)
        {
            Function = function;
            Accumulator = accumulator;
            List = list;
        }
    }

    // Command class for function definitions
    public class DefineFunctionCommandAst : CommandAst
    {
        public string Name { get; set; }
        public List<string> Params { get; set; }
        public CommandAst Body { get; set; }

        public DefineFunctionCommandAst(string name, List<string> parameters, CommandAst body)
        {
            Name = name;
            Params = parameters;
            Body = body;
        }
    }

    // Command class for try-catch expressions
    public class TryCatchCommandAst : CommandAst
    {
        public CommandAst TryBlock { get; set; }
        public CommandAst CatchBlock { get; set; }

        public TryCatchCommandAst(CommandAst tryBlock, CommandAst catchBlock)
        {
            TryBlock = tryBlock;
            CatchBlock = catchBlock;
        }
    }
}
