using BankApi.Application.DTOs.Auth;
using BankApi.Application.UseCases.Auth;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(Register register, Login login) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await register.ExecuteAsync(request);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await login.ExecuteAsync(request);
        if (result is null)
            return Unauthorized(new { Message = "Credenciales inválidas." });

        return Ok(result);
    }
}
