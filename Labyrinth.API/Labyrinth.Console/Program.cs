using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5232/chat", options => {
        options.AccessTokenProvider = () => Task.FromResult("eyJhbGciOiJSUzI1NiIsImtpZCI6ImQ0MjY5YTE3MzBlNTA3MTllNmIxNjA2ZTQyYzNhYjMyYjEyODA0NDkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vbGFnZGFlbW9uLWdhbWUtYXV0aGVudGljYXRpb24iLCJhdWQiOiJsYWdkYWVtb24tZ2FtZS1hdXRoZW50aWNhdGlvbiIsImF1dGhfdGltZSI6MTcyMzk5OTQ1OSwidXNlcl9pZCI6IkIzb3FSQ2VBMzlnOWNxZWtjbnRKN0x3M2lGbjIiLCJzdWIiOiJCM29xUkNlQTM5ZzljcWVrY250SjdMdzNpRm4yIiwiaWF0IjoxNzIzOTk5NDU5LCJleHAiOjE3MjQwMDMwNTksImVtYWlsIjoid3dlYXRsYWtlMTIzNEBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiZmlyZWJhc2UiOnsiaWRlbnRpdGllcyI6eyJlbWFpbCI6WyJ3d2VhdGxha2UxMjM0QGdtYWlsLmNvbSJdfSwic2lnbl9pbl9wcm92aWRlciI6InBhc3N3b3JkIn19.sy3yNqIemz5PzV04CGVp3I0XDOv1M3xeQ0dWMJOfyjGjRP6euHFJWxwVlI2ma_2EzrJhihx8aZLGdnyBHTiH8MOSk7UHhAC_I1rq9UHK58UFhSP2hbE4ot14cKng - ifPUzGnUbxQZ_kYV6n0HhQvVBSn2aIEcW6FElCBoAljgV7tfTBjxlb5at8Wv - 11l_8ra4pHKQ - DBnf47YdNrUbhZ - BGEs1C9rNCgBA56G59y7MpXWkJX_dqY_5Men2sVaL9BFRhcxUUwji1yinJdXgmQAsztKK7QmuGYsjkXOtWJV6efbaEX67rV8Lx9phNom88NrcEVxteE4t5hCzpRjMbVQ");
    })
    .ConfigureLogging(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Debug);
        logging.AddConsole();
    })
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user}: {message}");
});

await connection.StartAsync();

Console.WriteLine("Connected to SignalR Hub. Type a message and press Enter to send.");

while (true)
{
    var message = Console.ReadLine();
    await connection.InvokeAsync("SendMessage", "Lobby", "Owner", message);
}
