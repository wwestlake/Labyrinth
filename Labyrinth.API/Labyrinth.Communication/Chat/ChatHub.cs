using Microsoft.AspNetCore.SignalR;

namespace Labyrinth.Communication.Chat
{
    public class ChatHub : Hub
    {
        private readonly ChatBotService _chatBotService;

        // Inject ChatBotService through the constructor
        public ChatHub(ChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        // Join a room (e.g., a room chat channel)
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ReceiveMessage", "System", $"A new user has joined the room {roomId}.");

            // Optional: Notify the chatbot about joining a room if needed
        }

        // Leave a room
        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("ReceiveMessage", "System", $"A user has left the room {roomId}.");
        }

        // Send a message to a specific channel (room or role-based channel)
        public async Task SendMessage(string channelId, string user, string message)
        {
            // Broadcast the user's message to the room
            await Clients.Group(channelId).SendAsync("ReceiveMessage", user, message);

            // Monitor messages using ChatBotService for moderation and assistance
            await _chatBotService.MonitorMessages(channelId, user, message);
        }

        // Send a notice to the general chat channel
        public async Task SendNotice(string message)
        {
            await Clients.Group("General").SendAsync("ReceiveMessage", "Admin", message);
        }

        // Handle client disconnection (optional)
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Logic to handle cleanup on client disconnection
            await base.OnDisconnectedAsync(exception);
        }
    }
}
