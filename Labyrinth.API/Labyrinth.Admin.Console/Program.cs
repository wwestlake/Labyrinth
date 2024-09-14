using System;
using Labyrinth.Admin;



AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
{
    Console.WriteLine($"Unhandled exception: {args.ExceptionObject}");
    if (args.ExceptionObject is Exception ex)
    {
        Console.WriteLine($"Exception message: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
    }
};


// Program.cs (Top-Level Statements)
Console.WriteLine("Starting Labyrinth Admin Console...");

// Initialize and start the LabyrinthClient
var labyrinthClient = new LabyrinthClient();
await labyrinthClient.StartClient();


Console.WriteLine("Exiting Labyrinth Admin Console...");

// Application continues based on the REPL loop managed by LabyrinthClient
