using EchoChatAPI.Models;
using EchoChatAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EchoChatApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = new AuthService();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var success = _authService.Login(request.Username, request.Password);
        if (success)
            return Ok(new { message = "Login successful" });
        return Unauthorized(new { message = "Invalid username or password" });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        var success = _authService.Register(request.Email, request.Username, request.Password);
        if (success)
            return Ok(new { message = "Registration successful" });
        return BadRequest(new { message = "Username already exists" });
    }
}