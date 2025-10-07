using EchoChatApi.Models;
using EchoChatApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EchoChatApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly MessageService _messageService;

    public MessageController(MessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("message")]
    public IActionResult PostMessage([FromBody] MessageRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Content))
            return BadRequest(new { message = "Invalid message data." });
        _messageService.SaveMessage(request.Username, request.Content);
        return Ok(new { message = "Message saved successfully." });
    }

    [HttpGet("messages")]
    public IActionResult GetMessages()
    {
        var messages = _messageService.GetMessages();
        return Ok(messages);
    }
}