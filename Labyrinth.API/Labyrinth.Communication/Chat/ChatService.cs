using Microsoft.AspNetCore.SignalR;

namespace Labyrinth.Communication.Chat
{
    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task JoinRoom(string connectionId, string roomId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, roomId);
            await _hubContext.Clients.Group(roomId).SendAsync("ReceiveMessage", "System", $"A new user has joined the room {roomId}.");
        }

        public async Task LeaveRoom(string connectionId, string roomId)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, roomId);
            await _hubContext.Clients.Group(roomId).SendAsync("ReceiveMessage", "System", $"A user has left the room {roomId}.");
        }

        public async Task SendMessage(string channelId, string user, string message)
        {
            await _hubContext.Clients.Group(channelId).SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendNotice(string message)
        {
            await _hubContext.Clients.Group("General").SendAsync("ReceiveMessage", "Admin", message);
        }
    }
}
