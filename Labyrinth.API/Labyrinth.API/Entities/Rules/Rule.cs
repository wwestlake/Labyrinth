namespace Labyrinth.API.Entities.Rules
{
    public class Rule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Condition> Conditions { get; set; }
        public List<Action> Actions { get; set; }
    }
}
