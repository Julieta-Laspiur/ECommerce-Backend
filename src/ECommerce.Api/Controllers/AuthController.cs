using ECommerce.Application.UseCases.Users.Commands;
using ECommerce.Application.UseCases.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            await _mediator.Send(new RegisterUserCommand(request));

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
        var response = await _mediator.Send(new LoginCommand(request));

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
