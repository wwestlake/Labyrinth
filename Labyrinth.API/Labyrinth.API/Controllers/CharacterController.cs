using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly LabyrinthDbContext _context;

    public CharacterController(LabyrinthDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Character>> Get()
    {
        return _context.Characters.ToList();
    }
}
