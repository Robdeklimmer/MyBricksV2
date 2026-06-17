using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyBricks.Application.Features.Auth.Commands;
using MyBricks.Application.Features.Auth.DTOs;

namespace MyBricks.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterCommand command)
    {
        return await _mediator.Send(command);
    }
}
