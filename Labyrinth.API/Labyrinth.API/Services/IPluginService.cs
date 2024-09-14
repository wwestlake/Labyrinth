using Labyrinth.API.Entities.Plugins;
using MongoDB.Bson;
using System;
using System.Linq;

namespace Labyrinth.API.Services
{
    public interface IPluginService
    {
        /// <summary>
        /// Compiles the code from a CodeStore and, if successful, creates a PluginStore with the compiled assembly.
        /// </summary>
        /// <param name="codeStoreId">The ObjectId of the CodeStore to compile.</param>
        /// <returns>A Task that returns a PluginStore if successful or null if there was an error.</returns>
        Task<(PluginStore pluginStore, string[] errors)> CompileCodeStoreAsync(ObjectId codeStoreId);

        /// <summary>
        /// Creates a new code store and inserts it into the MongoDB collection.
        /// </summary>
        /// <param name="codeStore">The code store to insert.</param>
        /// <returns>The inserted code store.</returns>
        Task<CodeStore> CreateCodeStoreAsync(CodeStore codeStore);

        /// <summary>
        /// Creates a new plugin and inserts it into the MongoDB collection.
        /// </summary>
        /// <param name="plugin">The plugin to insert.</param>
        /// <returns>The inserted plugin.</returns>
        Task<PluginStore> CreatePluginAsync(PluginStore plugin);

        /// <summary>
        /// Deletes a code store from the MongoDB collection by its ObjectId.
        /// </summary>
        /// <param name="id">The ObjectId of the code store to delete.</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        Task<bool> DeleteCodeStoreAsync(ObjectId id);

        /// <summary>
        /// Deletes a plugin from the MongoDB collection by its ObjectId.
        /// </summary>
        /// <param name="id">The ObjectId of the plugin to delete.</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        Task<bool> DeletePluginAsync(ObjectId id);

        /// <summary>
        /// Retrieves all code stores.
        /// </summary>
        /// <returns>A list of all code stores.</returns>
        Task<List<CodeStore>> GetAllCodeStoresAsync();

        /// <summary>
        /// Retrieves all plugins.
        /// </summary>
        /// <returns>A list of all plugins.</returns>
        Task<List<PluginStore>> GetAllPluginsAsync();

        /// <summary>
        /// Retrieves a code store by its ObjectId.
        /// </summary>
        /// <param name="id">The ObjectId of the code store.</param>
        /// <returns>The code store, or null if not found.</returns>
        Task<CodeStore> GetCodeStoreByIdAsync(ObjectId id);

        /// <summary>
        /// Retrieves a plugin by its ObjectId.
        /// </summary>
        /// <param name="id">The ObjectId of the plugin.</param>
        /// <returns>The plugin, or null if not found.</returns>
        Task<PluginStore> GetPluginByIdAsync(ObjectId id);

        /// <summary>
        /// Updates a code store in the MongoDB collection.
        /// </summary>
        /// <param name="id">The ObjectId of the code store to update.</param>
        /// <param name="updatedCodeStore">The updated code store data.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> UpdateCodeStoreAsync(ObjectId id, CodeStore updatedCodeStore);

        /// <summary>
        /// Updates a plugin in the MongoDB collection.
        /// </summary>
        /// <param name="id">The ObjectId of the plugin to update.</param>
        /// <param name="updatedPlugin">The updated plugin data.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        Task<bool> UpdatePluginAsync(ObjectId id, PluginStore updatedPlugin);
    }
}
