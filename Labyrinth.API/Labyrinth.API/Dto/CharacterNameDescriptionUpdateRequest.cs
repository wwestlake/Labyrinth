namespace Labyrinth.API.Dto
{
    // Request model for updating a character's name and description
    public class CharacterNameDescriptionUpdateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
