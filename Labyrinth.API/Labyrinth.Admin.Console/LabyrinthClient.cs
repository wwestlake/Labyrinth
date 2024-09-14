using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Text.Json;
using Labyrinth.API.Controllers;

namespace Labyrinth.Admin;

public class LabyrinthClient
{
    private readonly HttpClient _httpClient;
    private string _authToken;
    private bool _isAuthenticated;
    private bool _replLoop = true;
    private string _userEmail;

    public LabyrinthClient()
    {
        // Create an HTTP handler that uses the custom certificate
        var handler = new HttpClientHandler();

        // Load your certificate
        var certificatePath = @"D:\Projects\Labyrinth\secrets\localhost.crt"; // Path to your certificate
        var certificateKeyPath = @"D:\Projects\Labyrinth\secrets\localhost.key"; // Path to your private key

        // Create an X509Certificate2 object
        var cert = new X509Certificate2(certificatePath, "htrv5gpl"); // Replace with your actual certificate password if needed

        handler.ClientCertificates.Add(cert);
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true; // This bypasses SSL certificate validation (for development only)

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:5001/") // Example base address
        };

        // Set default request headers
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("LabyrinthAdminClient/1.0");

        Console.WriteLine("HttpClient initialized with base address: https://localhost:5001/");
    }

    public async Task StartClient()
    {
        Console.WriteLine("Welcome to the Labyrinth Admin Console!");
        Console.WriteLine("Type 'help' to see a list of commands.");
        await AuthenticateUser();

        while (_replLoop)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            await HandleInput(input);
        }
    }

    private async Task AuthenticateUser()
    {
        Console.WriteLine("Please log in.");
        Console.Write("Email: ");
        _userEmail = Console.ReadLine();
        Console.Write("Password: ");
        var password = ReadPassword();

        Console.WriteLine("Attempting to log in...");

        var loginResult = await Login(_userEmail, password);

        if (loginResult)
        {
            _isAuthenticated = true;
            Console.WriteLine("Login successful!");
        }
        else
        {
            Console.WriteLine("Login failed. Please try again.");
        }
    }

    private async Task<bool> Login(string email, string password)
    {
        try
        {
            var requestUri = "api/auth/login"; // Relative URL using the base address
            var requestBody = new
            {
                email = email,
                password = password,
                returnSecureToken = true
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            Console.WriteLine($"Sending POST request to {requestUri} with email: {email}");

            var response = await _httpClient.PostAsync(requestUri, content);

            Console.WriteLine($"Received response. Status Code: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<FirebaseLoginResponse>(responseContent);
                _authToken = result.IdToken;

                // Set the authorization header for future requests
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
                Console.WriteLine("Token received and authorization header set.");

                return true;
            }
            else
            {
                Console.WriteLine($"Failed to log in. Server responded with an error. {response.Content.ToString()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during login: {ex.Message}");
        }

        return false;
    }

    private async Task HandleInput(string input)
    {
        input = input.Trim().ToLower();

        switch (input)
        {
            case "help":
                DisplayHelp();
                break;
            case "exit":
                _replLoop = false;
                Console.WriteLine("Exiting Labyrinth Admin Console. Goodbye!");
                break;
            default:
                await ExecuteCommand(input);
                break;
        }
    }

    private void DisplayHelp()
    {
        Console.WriteLine("Available commands:");
        Console.WriteLine("help - Display this help message");
        Console.WriteLine("exit - Exit the application");
        // Add more commands as needed
    }

    private async Task ExecuteCommand(string command)
    {
        Console.WriteLine($"Executing command: {command}");
        // Example: Call backend API based on command
    }

    private async Task RefreshTokenIfNeeded()
    {
        // Logic to refresh token if needed
    }

    private string ReadPassword()
    {
        StringBuilder password = new StringBuilder();
        while (true)
        {
            var key = Console.ReadKey(intercept: true);
            if (key.Key == ConsoleKey.Enter)
                break;
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                password.Length--;
            else if (!char.IsControl(key.KeyChar))
                password.Append(key.KeyChar);
        }
        Console.WriteLine();
        return password.ToString();
    }
}
