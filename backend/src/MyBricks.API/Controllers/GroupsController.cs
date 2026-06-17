using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBricks.Application.Features.FamilyGroups.Commands;
using MyBricks.Application.Features.FamilyGroups.DTOs;
using MyBricks.Application.Features.FamilyGroups.Queries;

namespace MyBricks.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FamilyGroupDto>>> GetMyGroups()
    {
        return Ok(await _mediator.Send(new GetMyFamilyGroupsQuery()));
    }

    [HttpPost]
    public async Task<ActionResult<FamilyGroupDto>> Create(CreateFamilyGroupCommand command)
    {
        return await _mediator.Send(command);
    }

    [HttpPost("join")]
    public async Task<ActionResult<FamilyGroupDto>> Join(JoinFamilyGroupCommand command)
    {
        return await _mediator.Send(command);
    }
}
