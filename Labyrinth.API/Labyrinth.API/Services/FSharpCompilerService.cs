using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Labyrinth.Lang;

namespace Labyrinth.API.Services
{
    public class FSharpCompilerService : IFSharpCompilerService
    {
        private readonly FSharpCompiler _compiler;

        public FSharpCompilerService()
        {
            _compiler = new FSharpCompiler();
        }

        public (Assembly, byte[] rawBytes, List<string>) CompileFromString(string fsharpCode, List<string> assemblyReferences = null)
        {
            return _compiler.CompileFromString(fsharpCode, assemblyReferences);
        }

        public (Assembly, byte[] rawBytes, List<string>) CompileFromFile(string filePath, List<string> assemblyReferences = null)
        {
            return _compiler.CompileFromFile(filePath, assemblyReferences);
        }

        public (Assembly, byte[] rawBytes, List<string>) CompileFromStream(Stream inputStream, List<string> assemblyReferences = null)
        {
            return _compiler.CompileFromStream(inputStream, assemblyReferences);
        }

        public (Assembly, byte[] rawBytes, List<string>) CompileFromFiles(List<string> filePaths, List<string> assemblyReferences = null)
        {
            return _compiler.CompileFromFiles(filePaths, assemblyReferences);
        }

        // New method for compiling from multiple strings (code snippets)
        public (Assembly, byte[] rawBytes, List<string>) CompileFromStrings(List<string> codeSnippets, List<string> assemblyReferences = null)
        {
            return _compiler.CompileFromFiles(codeSnippets, assemblyReferences);
        }
    }
}
