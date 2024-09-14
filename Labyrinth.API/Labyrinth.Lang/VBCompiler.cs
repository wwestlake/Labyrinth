using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.VisualBasic;

namespace Labyrinth.Lang
{
    public class VBCompiler : ILanguageCompiler
    {
        public (byte[] compiledAssembly, List<string> errors) Compile(List<string> sourceCode)
        {
            // Create syntax trees from the provided source code files
            var syntaxTrees = sourceCode.Select(code => VisualBasicSyntaxTree.ParseText(code)).ToList();

            // Add references to necessary assemblies, including Microsoft.VisualBasic and System.Console
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location), // mscorlib
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location), // System.Console
                MetadataReference.CreateFromFile(AppDomain.CurrentDomain.GetAssemblies()
                    .First(a => a.GetName().Name == "System.Runtime").Location) // System.Runtime
            };

            // Safely attempt to reference Microsoft.VisualBasic assembly
            var vbAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Microsoft.VisualBasic");

            if (vbAssembly != null)
            {
                references.Add(MetadataReference.CreateFromFile(vbAssembly.Location)); // Add Microsoft.VisualBasic if found
            }
            else
            {
                // Handle the case where Microsoft.VisualBasic is not found
                return (null, new List<string> { "Microsoft.VisualBasic assembly not found." });
            }

            // Create the compilation object for VB.NET
            var compilation = VisualBasicCompilation.Create(
                "CompiledVBAssembly",
                syntaxTrees,
                references,
                new VisualBasicCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
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
            return "VisualBasic";
        }
    }
}
