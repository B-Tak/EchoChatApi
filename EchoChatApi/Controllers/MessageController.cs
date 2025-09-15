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

    [HttpPost]
    public IActionResult PostMessage([FromBody] MessageRequest request)
    {
        if (request == null || request.UserId <= 0 || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(new { message = "Invalid message data." });
        _messageService.SaveMessage(request.UserId, request.Message);
        return Ok(new { message = "Message saved successfully." });
    }

    [HttpGet]
    public IActionResult GetMessages()
    {
        var messages = _messageService.GetMessages();
        return Ok(messages);
    }
}