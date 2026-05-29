using ECommerce.Application.UseCases.Users.Commands;
using ECommerce.Application.UseCases.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRegisterUserUseCase _registerUserUseCase;
    private readonly ILoginUseCase _loginUseCase;

    public AuthController(
        IRegisterUserUseCase registerUserUseCase,
        ILoginUseCase loginUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _loginUseCase = loginUseCase;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            await _registerUserUseCase.ExecuteAsync(
                new RegisterUserCommand(request));

            return Ok(new
            {
                message = "User created"
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _loginUseCase.ExecuteAsync(
            new LoginCommand(request));

        if (response is null)
        {
            return Unauthorized(new
            {
                message = "Invalid credentials"
            });
        }

        return Ok(response);
    }
}
