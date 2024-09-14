using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Labyrinth.API.Entities.Plugins;
using Labyrinth.Lang;

namespace Labyrinth.API.Services
{
    /// <summary>
    /// Service for managing plugins stored in MongoDB and handling compilation of code.
    /// </summary>
    public class PluginService : IPluginService
    {
        private readonly IMongoCollection<PluginStore> _pluginCollection;
        private readonly IMongoCollection<CodeStore> _codeCollection;
        private readonly ICompilerFactory _compilerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginService"/> class.
        /// </summary>
        /// <param name="mongoClient">The MongoDB client to be injected.</param>
        /// <param name="compilerFactory">The compiler factory to get the appropriate compiler.</param>
        public PluginService(IMongoClient mongoClient, ICompilerFactory compilerFactory)
        {
            var database = mongoClient.GetDatabase("Labyrinth"); // Use your actual database name
            _pluginCollection = database.GetCollection<PluginStore>("Plugins");
            _codeCollection = database.GetCollection<CodeStore>("CodeStores");
            _compilerFactory = compilerFactory;  // CompilerFactory is injected
        }

        #region PluginStore CRUD

        public async Task<PluginStore> CreatePluginAsync(PluginStore plugin)
        {
            await _pluginCollection.InsertOneAsync(plugin);
            return plugin;
        }

        public async Task<PluginStore> GetPluginByIdAsync(ObjectId id)
        {
            var filter = Builders<PluginStore>.Filter.Eq(p => p.Id, id);
            return await _pluginCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<PluginStore>> GetAllPluginsAsync()
        {
            return await _pluginCollection.Find(_ => true).ToListAsync();
        }

        public async Task<bool> UpdatePluginAsync(ObjectId id, PluginStore updatedPlugin)
        {
            var filter = Builders<PluginStore>.Filter.Eq(p => p.Id, id);
            var result = await _pluginCollection.ReplaceOneAsync(filter, updatedPlugin);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeletePluginAsync(ObjectId id)
        {
            var filter = Builders<PluginStore>.Filter.Eq(p => p.Id, id);
            var result = await _pluginCollection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        #endregion

        #region CodeStore CRUD

        public async Task<CodeStore> CreateCodeStoreAsync(CodeStore codeStore)
        {
            await _codeCollection.InsertOneAsync(codeStore);
            return codeStore;
        }

        public async Task<CodeStore> GetCodeStoreByIdAsync(ObjectId id)
        {
            var filter = Builders<CodeStore>.Filter.Eq(c => c.Id, id);
            return await _codeCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<CodeStore>> GetAllCodeStoresAsync()
        {
            return await _codeCollection.Find(_ => true).ToListAsync();
        }

        public async Task<bool> UpdateCodeStoreAsync(ObjectId id, CodeStore updatedCodeStore)
        {
            var filter = Builders<CodeStore>.Filter.Eq(c => c.Id, id);
            var result = await _codeCollection.ReplaceOneAsync(filter, updatedCodeStore);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteCodeStoreAsync(ObjectId id)
        {
            var filter = Builders<CodeStore>.Filter.Eq(c => c.Id, id);
            var result = await _codeCollection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        #endregion

        #region Compilation

        /// <summary>
        /// Compiles the code from a CodeStore and, if successful, creates a PluginStore with the compiled assembly.
        /// </summary>
        /// <param name="codeStoreId">The ObjectId of the CodeStore to compile.</param>
        /// <returns>A Task that returns a PluginStore if successful or null if there was an error.</returns>
        public async Task<(PluginStore pluginStore, string[] errors)> CompileCodeStoreAsync(ObjectId codeStoreId)
        {
            // Retrieve the CodeStore by Id
            var codeStore = await GetCodeStoreByIdAsync(codeStoreId);
            if (codeStore == null)
            {
                return (null, new[] { "CodeStore not found." });
            }

            // Get the correct compiler based on the language in the CodeStore
            var compiler = _compilerFactory.GetCompiler(codeStore.Language);
            if (compiler == null)
            {
                return (null, new[] { $"No compiler found for language: {codeStore.Language}" });
            }

            // Prepare the source code for compilation (list of code from CodeFiles)
            var sourceCodeList = codeStore.GetCodeFiles().Select(cf => cf.Code).ToList();

            // Compile the code using the appropriate compiler
            var (bytes, errors) = compiler.Compile(sourceCodeList);

            if (bytes == null)
            {
                // Return errors if compilation failed
                return (null, errors.ToArray());
            }

            // Find the latest version of the plugin to set the next version number
            var latestPlugin = await _pluginCollection.Find(p => p.Name == codeStore.Name)
                                                      .SortByDescending(p => p.Version)
                                                      .FirstOrDefaultAsync();

            // Increment version
            string nextVersion = latestPlugin != null ? IncrementVersion(latestPlugin.Version) : "1.0.0";

            // Create a new PluginStore object with the compiled assembly
            var pluginStore = new PluginStore(
                bytes,
                codeStore.Name,
                codeStore.Description,
                nextVersion,
                codeStore.UserId
            );

            // Insert the new PluginStore into the MongoDB collection
            await _pluginCollection.InsertOneAsync(pluginStore);

            // Return the successfully created plugin
            return (pluginStore, null);
        }

        /// <summary>
        /// Helper method to increment the version number.
        /// </summary>
        /// <param name="currentVersion">The current version as a string.</param>
        /// <returns>The incremented version string.</returns>
        private string IncrementVersion(string currentVersion)
        {
            // Split the version number into major, minor, and patch
            var versionParts = currentVersion.Split('.');
            if (versionParts.Length != 3)
            {
                throw new InvalidOperationException("Invalid version format.");
            }

            int major = int.Parse(versionParts[0]);
            int minor = int.Parse(versionParts[1]);
            int patch = int.Parse(versionParts[2]);

            // Increment patch version
            patch++;

            return $"{major}.{minor}.{patch}";
        }

        #endregion
    }
}
