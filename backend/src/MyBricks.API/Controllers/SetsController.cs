using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBricks.Application.Features.Sets.Commands;
using MyBricks.Application.Features.Sets.DTOs;
using MyBricks.Application.Features.Sets.Queries;

namespace MyBricks.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserSetDto>>> GetMySets([FromQuery] int? familyGroupId)
    {
        return Ok(await _mediator.Send(new GetMySetsQuery(familyGroupId)));
    }

    [HttpPost]
    public async Task<ActionResult<UserSetDto>> AddSet(AddSetCommand command)
    {
        return await _mediator.Send(command);
    }
}
