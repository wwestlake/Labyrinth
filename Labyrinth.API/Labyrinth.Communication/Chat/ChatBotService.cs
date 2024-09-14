using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Labyrinth.Communication.Chat
{
    public class ChatBotService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly HttpClient _httpClient;
        private readonly string _openAiApiKey;
        private readonly string _botUserName;

        public ChatBotService(IHubContext<ChatHub> hubContext, HttpClient httpClient, string openAiApiKey, string botUserName = "HelpBot")
        {
            _hubContext = hubContext;
            _httpClient = httpClient;
            _openAiApiKey = openAiApiKey;
            _botUserName = botUserName;
        }

        public async Task MonitorMessages(string roomId, string user, string message)
        {
            // Check for inappropriate content and handle moderation
            if (IsMessageInappropriate(message))
            {
                await ModerateMessage(roomId, user, message);
            }
            else
            {
                await ProvideHelp(roomId, user, message);
            }
        }

        private bool IsMessageInappropriate(string message)
        {
            // Implement your logic for detecting inappropriate content
            // This could involve keyword checking, sentiment analysis, etc.
            // For this example, we'll keep it simple:
            return message.Contains("badword"); // Example of simple keyword check
        }

        private async Task ModerateMessage(string roomId, string user, string message)
        {
            string warningMessage = $"[Moderation] Your message violates our community guidelines, {user}. Please adhere to the rules.";
            await _hubContext.Clients.Group(roomId).SendAsync("ReceiveMessage", _botUserName, warningMessage);
        }

        private async Task ProvideHelp(string roomId, string user, string message)
        {
            var response = await GetChatBotResponse(message);
            if (!string.IsNullOrEmpty(response))
            {
                await _hubContext.Clients.Group(roomId).SendAsync("ReceiveMessage", _botUserName, response);
            }
        }

        private async Task<string> GetChatBotResponse(string message)
        {
            var request = new
            {
                model = "gpt-4",
                messages = new List<object>
                {
                    new { role = "system", content = "You are a helpful assistant and moderator in a chat room. Provide clear, concise responses to help users understand how to use the system or answer their questions." },
                    new { role = "user", content = message }
                },
                max_tokens = 150
            };

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Headers =
                {
                    { "Authorization", $"Bearer {_openAiApiKey}" }
                },
                Content = JsonContent.Create(request)
            };

            var response = await _httpClient.SendAsync(httpRequestMessage);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<OpenAiResponse>(jsonResponse);
                return responseData?.Choices?[0]?.Message?.Content;
            }

            return "I'm sorry, I couldn't process your request at the moment.";
        }
    }

    // Helper class to parse OpenAI API response
    public class OpenAiResponse
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }

        public class Choice
        {
            [JsonPropertyName("message")]
            public Message Message { get; set; }
        }

        public class Message
        {
            [JsonPropertyName("content")]
            public string Content { get; set; }
        }
    }
}
