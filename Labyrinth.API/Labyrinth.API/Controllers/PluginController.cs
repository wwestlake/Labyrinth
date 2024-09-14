using Labyrinth.API.Entities.Plugins;
using Labyrinth.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Labyrinth.API.Controllers;

[Authorize(Roles = "Admin,Owner")]
[Route("api/[controller]")]
[ApiController]
public class PluginController : ControllerBase
{
    private readonly IPluginService _pluginService;

    public PluginController(IPluginService pluginService)
    {
        _pluginService = pluginService;
    }

    #region PluginStore Endpoints

    /// <summary>
    /// Retrieves all plugins.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<PluginStore>>> GetAllPlugins()
    {
        var plugins = await _pluginService.GetAllPluginsAsync();
        return Ok(plugins);
    }

    /// <summary>
    /// Retrieves a specific plugin by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PluginStore>> GetPluginById(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var plugin = await _pluginService.GetPluginByIdAsync(objectId);

        if (plugin == null)
        {
            return NotFound();
        }

        return Ok(plugin);
    }

    /// <summary>
    /// Creates a new plugin.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PluginStore>> CreatePlugin([FromBody] PluginStore plugin)
    {
        var createdPlugin = await _pluginService.CreatePluginAsync(plugin);
        return CreatedAtAction(nameof(GetPluginById), new { id = createdPlugin.Id.ToString() }, createdPlugin);
    }

    /// <summary>
    /// Updates an existing plugin.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePlugin(string id, [FromBody] PluginStore plugin)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var isUpdated = await _pluginService.UpdatePluginAsync(objectId, plugin);
        if (!isUpdated)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a plugin by ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlugin(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var isDeleted = await _pluginService.DeletePluginAsync(objectId);
        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    #endregion

    #region CodeStore Endpoints

    /// <summary>
    /// Retrieves all code stores.
    /// </summary>
    [HttpGet("codeStores")]
    public async Task<ActionResult<List<CodeStore>>> GetAllCodeStores()
    {
        var codeStores = await _pluginService.GetAllCodeStoresAsync();
        return Ok(codeStores);
    }

    /// <summary>
    /// Retrieves a specific code store by ID.
    /// </summary>
    [HttpGet("codeStores/{id}")]
    public async Task<ActionResult<CodeStore>> GetCodeStoreById(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var codeStore = await _pluginService.GetCodeStoreByIdAsync(objectId);

        if (codeStore == null)
        {
            return NotFound();
        }

        return Ok(codeStore);
    }

    /// <summary>
    /// Creates a new code store.
    /// </summary>
    [HttpPost("codeStores")]
    public async Task<ActionResult<CodeStore>> CreateCodeStore([FromBody] CodeStore codeStore)
    {
        var createdCodeStore = await _pluginService.CreateCodeStoreAsync(codeStore);
        return CreatedAtAction(nameof(GetCodeStoreById), new { id = createdCodeStore.Id.ToString() }, createdCodeStore);
    }

    /// <summary>
    /// Updates an existing code store.
    /// </summary>
    [HttpPut("codeStores/{id}")]
    public async Task<ActionResult> UpdateCodeStore(string id, [FromBody] CodeStore codeStore)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var isUpdated = await _pluginService.UpdateCodeStoreAsync(objectId, codeStore);
        if (!isUpdated)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Deletes a code store by ID.
    /// </summary>
    [HttpDelete("codeStores/{id}")]
    public async Task<ActionResult> DeleteCodeStore(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var isDeleted = await _pluginService.DeleteCodeStoreAsync(objectId);
        if (!isDeleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    #endregion

    #region Compilation Endpoint

    /// <summary>
    /// Compiles a code store and creates a new plugin if successful.
    /// </summary>
    [HttpPost("codeStores/{id}/compile")]
    public async Task<ActionResult<PluginStore>> CompileCodeStore(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId objectId))
        {
            return BadRequest("Invalid ID format.");
        }

        var (pluginStore, errors) = await _pluginService.CompileCodeStoreAsync(objectId);

        if (errors != null)
        {
            return BadRequest(new { Errors = errors });
        }

        return Ok(pluginStore);
    }

    #endregion
}
