namespace EchoChatApi.Models;

public class MessageResponse
{
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}

