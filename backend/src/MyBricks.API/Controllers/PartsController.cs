using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBricks.Application.Features.Parts.Commands;
using MyBricks.Application.Features.Parts.DTOs;

namespace MyBricks.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("flag-missing")]
    public async Task<ActionResult<MissingPartDto>> FlagMissing(FlagMissingPartCommand command)
    {
        return await _mediator.Send(command);
    }
}
