namespace Labyrinth.Common;

// Base abstract class for all command types
public abstract class CommandAst { }

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


