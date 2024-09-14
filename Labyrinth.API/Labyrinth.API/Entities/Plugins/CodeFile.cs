using System;

namespace Labyrinth.API.Entities.Plugins
{
    /// <summary>
    /// Represents a single source code file with a name and code content.
    /// </summary>
    public class CodeFile
    {
        /// <summary>
        /// Gets or sets the name of the code file (similar to a filename).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content of the code file (source code).
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeFile"/> class.
        /// </summary>
        /// <param name="name">The name of the code file.</param>
        /// <param name="code">The source code content of the file.</param>
        public CodeFile(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}
