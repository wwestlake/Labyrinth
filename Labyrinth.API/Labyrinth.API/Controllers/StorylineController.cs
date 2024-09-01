using Labyrinth.API.Entities.Storyline;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Labyrinth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StorylineController : ControllerBase
    {
        private readonly IStorylineService _storylineService;

        public StorylineController(IStorylineService storylineService)
        {
            _storylineService = storylineService;
        }

        [HttpGet("chapters")]
        public async Task<ActionResult<List<Chapter>>> GetChapters()
        {
            var chapters = await _storylineService.GetChaptersAsync();
            return Ok(chapters);
        }

        [HttpGet("chapters/{id}")]
        public async Task<ActionResult<Chapter>> GetChapterById(string id)
        {
            var chapter = await _storylineService.GetChapterByIdAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            return Ok(chapter);
        }

        [HttpPost("chapters")]
        public async Task<ActionResult> CreateChapter([FromBody] Chapter chapter)
        {
            await _storylineService.CreateChapterAsync(chapter);
            return CreatedAtAction(nameof(GetChapterById), new { id = chapter.Id }, chapter);
        }

        [HttpPut("chapters/{id}")]
        public async Task<ActionResult> UpdateChapter(string id, [FromBody] Chapter updatedChapter)
        {
            var chapter = await _storylineService.GetChapterByIdAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            await _storylineService.UpdateChapterAsync(id, updatedChapter);
            return NoContent();
        }

        [HttpDelete("chapters/{id}")]
        public async Task<ActionResult> DeleteChapter(string id)
        {
            var chapter = await _storylineService.GetChapterByIdAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            await _storylineService.DeleteChapterAsync(id);
            return NoContent();
        }

        // Implement similar endpoints for other entities (Character, Event, PlayerAction, UserProgress)
    }
}
