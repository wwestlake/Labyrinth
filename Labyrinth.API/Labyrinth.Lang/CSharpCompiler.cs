using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace Labyrinth.Lang
{
    public class CSharpCompiler : ILanguageCompiler
    {
        public (byte[] compiledAssembly, List<string> errors) Compile(List<string> sourceCode)
        {
            // Create syntax trees from the provided source code files
            var syntaxTrees = sourceCode.Select(code => CSharpSyntaxTree.ParseText(code)).ToList();

            // Add references to common assemblies (you can add more as needed)
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(AppDomain.CurrentDomain.GetAssemblies()
                    .First(a => a.GetName().Name == "System.Runtime").Location)
            };

            // Create the compilation object
            var compilation = CSharpCompilation.Create(
                "CompiledCSharpAssembly",
                syntaxTrees,
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            // Prepare a memory stream to hold the compiled assembly
            using var ms = new MemoryStream();
            EmitResult result = compilation.Emit(ms);

            // Collect any errors or warnings
            List<string> errors = new List<string>();
            if (!result.Success)
            {
                foreach (var diagnostic in result.Diagnostics.Where(diag => diag.Severity == DiagnosticSeverity.Error))
                {
                    errors.Add(diagnostic.ToString());
                }
                return (null, errors); // Return null for the assembly if compilation failed
            }

            // If compilation is successful, return the compiled assembly as byte array
            ms.Seek(0, SeekOrigin.Begin);
            return (ms.ToArray(), errors);
        }

        public string GetLanguageName()
        {
            return "CSharp";
        }
    }
}
