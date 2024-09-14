using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using FSharp.Compiler;
using FSharp.Compiler.CodeAnalysis;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using System.Collections.Generic;
using System.Linq;

namespace Labyrinth.Lang
{
    public class FSharpCompiler : ILanguageCompiler
    {
        private readonly string _cacheDirectory;

        public FSharpCompiler(string cacheDirectory = null)
        {
            // Optionally set a cache directory for storing compiled assemblies
            _cacheDirectory = cacheDirectory ?? Path.GetTempPath();
        }

        // Compile F# code from a string with optional assembly references
        public (Assembly assembly, byte[] rawBytes, List<string> errors) CompileFromString(string fsharpCode, List<string> assemblyReferences = null, string assemblyName = "CompiledAssembly")
        {
            string tempFile = Path.Combine(_cacheDirectory, $"{assemblyName}.fs");
            File.WriteAllText(tempFile, fsharpCode);
            return CompileToAssembly(new List<string> { tempFile }, assemblyReferences, assemblyName);
        }

        // Compile F# code from a file stream with optional assembly references
        public (Assembly assembly, byte[] rawBytes, List<string> errors) CompileFromStream(Stream inputStream, List<string> assemblyReferences = null, string assemblyName = "CompiledAssembly")
        {
            string tempFile = Path.Combine(_cacheDirectory, $"{assemblyName}.fs");
            using (var fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
            {
                inputStream.CopyTo(fileStream);
            }
            return CompileToAssembly(new List<string> { tempFile }, assemblyReferences, assemblyName);
        }

        // Compile F# code from a single file path with optional assembly references
        public (Assembly assembly, byte[] rawBytes, List<string> errors) CompileFromFile(string filePath, List<string> assemblyReferences = null, string assemblyName = "CompiledAssembly")
        {
            return CompileToAssembly(new List<string> { filePath }, assemblyReferences, assemblyName);
        }

        // Compile multiple F# files into a single assembly with optional assembly references
        public (Assembly assembly, byte[] rawBytes, List<string> errors) CompileFromFiles(List<string> filePaths, List<string> assemblyReferences = null, string assemblyName = "CompiledAssembly")
        {
            return CompileToAssembly(filePaths, assemblyReferences, assemblyName);
        }

        // Compile F# code directly from a list of in-memory strings (no file involved)
        public (Assembly assembly, byte[] rawBytes, List<string> errors) CompileFromStrings(List<string> codeSnippets, List<string> assemblyReferences = null, string assemblyName = "CompiledAssembly")
        {
            // Generate unique names for each piece of code
            var tempFiles = codeSnippets.Select((code, index) =>
            {
                string tempFile = Path.Combine(_cacheDirectory, $"{assemblyName}_{index}.fs");
                File.WriteAllText(tempFile, code);
                return tempFile;
            }).ToList();

            return CompileToAssembly(tempFiles, assemblyReferences, assemblyName);
        }

        // Core method that handles compilation from a list of file paths or in-memory strings
        private (Assembly assembly, byte[] rawBytes, List<string> errors) CompileToAssembly(List<string> filePaths, List<string> assemblyReferences, string assemblyName)
        {
            // Create the FSharpChecker
            var checker = FSharpChecker.Create(
                projectCacheSize: FSharpOption<int>.None,
                keepAssemblyContents: FSharpOption<bool>.None,
                keepAllBackgroundResolutions: FSharpOption<bool>.None,
                legacyReferenceResolver: FSharpOption<LegacyReferenceResolver>.None,
                tryGetMetadataSnapshot: FSharpOption<FSharpFunc<Tuple<string, DateTime>, FSharpOption<Tuple<object, IntPtr, int>>>>.None,
                suggestNamesForErrors: FSharpOption<bool>.None,
                keepAllBackgroundSymbolUses: FSharpOption<bool>.None,
                enableBackgroundItemKeyStoreAndSemanticClassification: FSharpOption<bool>.None,
                enablePartialTypeChecking: FSharpOption<bool>.None,
                parallelReferenceResolution: FSharpOption<bool>.None,
                captureIdentifiersWhenParsing: FSharpOption<bool>.None,
                documentSource: FSharpOption<DocumentSource>.None,
                useSyntaxTreeCache: FSharpOption<bool>.None,
                useTransparentCompiler: FSharpOption<bool>.None
            );

            // Prepare F# compiler options
            string outputAssemblyPath = Path.Combine(_cacheDirectory, $"{assemblyName}.dll");
            var compileOptions = new List<string> { "fsc.exe", "-o", outputAssemblyPath, "-a" };
            compileOptions.AddRange(filePaths); // Add all the F# source files

            // Add references to other assemblies if provided
            if (assemblyReferences != null)
            {
                foreach (var reference in assemblyReferences)
                {
                    compileOptions.Add($"-r:{reference}"); // Add reference option for each assembly
                }
            }

            // Execute the FSharpAsync to get the result tuple
            var result = FSharpAsync.RunSynchronously(checker.Compile(compileOptions.ToArray(), FSharpOption<string>.None), null, null);

            // Access the tuple items (diagnostics and result code)
            var diagnostics = result.Item1; // FSharpDiagnostic[] (any errors or warnings)
            var exitCode = result.Item2;    // int (the result of the compilation)

            // List to store any compilation errors or warnings
            var errors = new List<string>();

            // Check if there are any diagnostics (errors or warnings)
            if (diagnostics.Length > 0)
            {
                foreach (var diagnostic in diagnostics)
                {
                    var message = $"{diagnostic.Severity}: {diagnostic.Message}";
                    if (diagnostic.StartLine != 0)
                    {
                        message += $" at line {diagnostic.StartLine}, column {diagnostic.StartColumn}";
                    }
                    errors.Add(message);
                }
            }

            // If there were errors or exit code is non-zero, return errors
            if (exitCode != 0 || errors.Any())
            {
                return (null, null, errors.Count > 0 ? errors : new List<string> { "Compilation failed." });
            }

            // Load the assembly into memory and return the raw bytes
            if (File.Exists(outputAssemblyPath))
            {
                var assembly = LoadAssembly(outputAssemblyPath);
                var rawBytes = File.ReadAllBytes(outputAssemblyPath);  // Get the raw bytes of the compiled assembly
                return (assembly, rawBytes, null);
            }
            else
            {
                return (null, null, new List<string> { "Failed to compile the assembly." });
            }
        }

        // Method to load the compiled assembly into memory
        private Assembly LoadAssembly(string assemblyPath)
        {
            var assemblyBytes = File.ReadAllBytes(assemblyPath);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(new MemoryStream(assemblyBytes));
            return assembly;
        }

        public (byte[] compiledAssembly, List<string> errors) Compile(List<string> sourceCode)
        {
            var (_, bytes, errors) = CompileFromStrings(sourceCode);
            return (bytes, errors);
        }

        public string GetLanguageName()
        {
            return "FSharp";
        }
    }
}
