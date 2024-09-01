using Labyrinth.API.Entities.Processes;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Labyrinth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator,Owner")] // Restrict access to Admins and Owners
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService _processService;

        public ProcessController(IProcessService processService)
        {
            _processService = processService;
        }

        // Create a new process
        [HttpPost]
        public async Task<IActionResult> CreateProcess([FromBody] Process process)
        {
            if (process == null)
            {
                return BadRequest("Process data is null.");
            }

            var createdProcess = await _processService.CreateProcessAsync(process);
            return CreatedAtAction(nameof(GetProcessById), new { id = createdProcess.Id.ToString() }, createdProcess);
        }

        // Retrieve a process by ID
        [HttpGet("{id:length(24)}", Name = "GetProcessById")]
        public async Task<IActionResult> GetProcessById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid process ID format.");
            }

            var process = await _processService.GetProcessByIdAsync(objectId);

            if (process == null)
            {
                return NotFound();
            }

            return Ok(process);
        }

        // Retrieve all processes
        [HttpGet]
        public async Task<IActionResult> GetAllProcesses()
        {
            var processes = await _processService.GetAllProcessesAsync();
            return Ok(processes);
        }

        // Update a process by ID
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateProcess(string id, [FromBody] Process updatedProcess)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid process ID format.");
            }

            var existingProcess = await _processService.GetProcessByIdAsync(objectId);

            if (existingProcess == null)
            {
                return NotFound();
            }

            updatedProcess.Id = objectId;
            var process = await _processService.UpdateProcessAsync(objectId, updatedProcess);

            if (process == null)
            {
                return NotFound();
            }

            return Ok(process);
        }

        // Delete a process by ID
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteProcess(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId))
            {
                return BadRequest("Invalid process ID format.");
            }

            var success = await _processService.DeleteProcessAsync(objectId);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
