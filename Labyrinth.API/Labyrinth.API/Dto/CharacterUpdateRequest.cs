namespace Labyrinth.API.Dto
{
    // Request model for updating a character's stats
    public class CharacterUpdateRequest
    {
        public IDictionary<string, int> UpdatedStats { get; set; }
    }
}
