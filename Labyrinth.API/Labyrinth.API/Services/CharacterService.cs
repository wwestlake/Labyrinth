public class CharacterService : ICharacterService
{
    private readonly LabyrinthDbContext _context;

    public CharacterService(LabyrinthDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Character> GetCharacters()
    {
        return _context.Characters.ToList();
    }
}
