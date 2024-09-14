using Labyrinth.Lang;
using System;

// Initialize the F# compiler
var compiler = new FSharpCompiler();

// Compile F# code from a string into an assembly
var assembly = compiler.CompileFromString(@"
    module HelloWorld

    let helloWorld = 
        ""Hello, World!""
");

// F# compiles the module as a static class
var type = assembly.GetType();

var property = type.GetProperty("helloWorld");

if (property == null)
{ 
    Console.WriteLine("Field not found");
    return;
}

// Access the static field 'helloWorld' from the compiled F# module
var helloWorldValue = property.GetValue(null);

// Print the value
Console.WriteLine(helloWorldValue);
