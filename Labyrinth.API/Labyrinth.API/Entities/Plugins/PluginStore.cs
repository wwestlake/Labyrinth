using System;
using System.IO;
using System.Reflection;
using MongoDB.Bson;

namespace Labyrinth.API.Entities.Plugins
{
    /// <summary>
    /// Represents a plugin stored in the MongoDB database, containing the compiled assembly and metadata.
    /// </summary>
    public class PluginStore
    {
        /// <summary>
        /// Gets or sets the unique identifier for the plugin.
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the plugin.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the plugin.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the version of the plugin.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the user ID of the person who compiled or uploaded the plugin.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets the byte array representing the compiled assembly.
        /// </summary>
        public byte[] Plugin { get; private set; }

        /// <summary>
        /// Gets or sets the date and time when the plugin was compiled or created.
        /// </summary>
        public DateTime CompiledAt { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginStore"/> class using the specified byte array and metadata.
        /// </summary>
        /// <param name="pluginBytes">The byte array representing the compiled assembly.</param>
        /// <param name="name">The name of the plugin.</param>
        /// <param name="description">The description of the plugin.</param>
        /// <param name="version">The version of the plugin.</param>
        /// <param name="userId">The user ID of the person who compiled the plugin.</param>
        public PluginStore(byte[] pluginBytes, string name, string description, string version, string userId)
        {
            Id = ObjectId.GenerateNewId();
            Name = name;
            Description = description;
            Version = version;
            UserId = userId;
            CompiledAt = DateTime.UtcNow;
            Plugin = pluginBytes; // Directly store the byte array from the compiler
        }

        /// <summary>
        /// Loads and returns the assembly from the stored byte array.
        /// </summary>
        /// <returns>The <see cref="Assembly"/> object created from the byte array.</returns>
        public Assembly GetAssemblyFromBytes()
        {
            using (var memoryStream = new MemoryStream(Plugin))
            {
                return Assembly.Load(memoryStream.ToArray());
            }
        }
    }
}
