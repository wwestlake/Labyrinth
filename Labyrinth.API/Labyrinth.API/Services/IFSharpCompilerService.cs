using System;
using System.Reflection;

namespace Labyrinth.API.Services
{
    public interface IFSharpCompilerService
    {
        (Assembly, byte[] rawBytes, List<string>) CompileFromString(string fsharpCode, List<string> assemblyReferences = null);
        (Assembly, byte[] rawBytes, List<string>) CompileFromStream(Stream inputStream, List<string> assemblyReferences = null);
        (Assembly, byte[] rawBytes, List<string>) CompileFromStrings(List<string> codeSnippets, List<string> assemblyReferences = null);
        (Assembly, byte[] rawBytes, List<string>) CompileFromFiles(List<string> filePaths, List<string> assemblyReferences = null);
    }
}
