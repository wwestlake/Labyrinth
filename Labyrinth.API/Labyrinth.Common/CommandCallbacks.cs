namespace Labyrinth.Common
{
    public class CommandCallbacks
    {
        public Action<CommandAst> OnSuccess { get; set; }
        public Action<string> OnFailure { get; set; }
    }
}
