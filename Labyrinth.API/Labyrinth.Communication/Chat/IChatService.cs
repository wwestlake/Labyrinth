namespace Labyrinth.Communication.Chat
{
    public interface IChatService
    {
        Task JoinRoom(string connectionId, string roomId);
        Task LeaveRoom(string connectionId, string roomId);
        Task SendMessage(string channelId, string user, string message);
        Task SendNotice(string message);
    }
}
