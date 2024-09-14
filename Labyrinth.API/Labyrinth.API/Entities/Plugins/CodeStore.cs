using System;
using System.Collections.Generic;
using MongoDB.Bson;
using Labyrinth.API.Entities;
using Labyrinth.Lang;

namespace Labyrinth.API.Entities.Plugins
{
    /// <summary>
    /// Represents a store for plugin source code, including metadata and multiple source code files.
    /// </summary>
    public class CodeStore
    {
        /// <summary>
        /// Gets or sets the unique identifier for the code store.
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
        /// Gets or sets the user ID of the person who created the plugin.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the code store was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the code store was last modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the list of code files representing the plugin's source code.
        /// </summary>
        public List<CodeFile> CodeFiles { get; set; } = new List<CodeFile>();

        /// <summary>
        /// Gets or sets the language the code is written in (e.g., CSharp, FSharp).
        /// </summary>
        public SupportedLanguages Language { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeStore"/> class.
        /// </summary>
        /// <param name="name">The name of the plugin.</param>
        /// <param name="description">The description of the plugin.</param>
        /// <param name="userId">The user ID of the person who created the plugin.</param>
        /// <param name="language">The programming language of the plugin's source code.</param>
        public CodeStore(string name, string description, string userId, SupportedLanguages language)
        {
            Id = ObjectId.GenerateNewId();
            Name = name;
            Description = description;
            UserId = userId;
            Created = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
            Language = language;
            CodeFiles = new List<CodeFile>();
        }

        /// <summary>
        /// Adds a new code file to the CodeFiles list.
        /// </summary>
        /// <param name="fileName">The name of the code file.</param>
        /// <param name="codeContent">The content of the code file.</param>
        public void AddCodeFile(string fileName, string codeContent)
        {
            var codeFile = new CodeFile(fileName, codeContent);
            CodeFiles.Add(codeFile);
            LastModified = DateTime.UtcNow;  // Update the last modified time
        }

        // Other existing methods...

        /// <summary>
        /// Gets the list of code files.
        /// </summary>
        /// <returns>The list of code files in order.</returns>
        public List<CodeFile> GetCodeFiles()
        {
            return new List<CodeFile>(CodeFiles);  // Return a copy to preserve encapsulation
        }
    }
}
