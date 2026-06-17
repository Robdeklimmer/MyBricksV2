using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBricks.Application.Features.ShoppingList.DTOs;
using MyBricks.Application.Features.ShoppingList.Queries;

namespace MyBricks.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ShoppingListController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShoppingListController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ShoppingListItemDto>>> GetShoppingList([FromQuery] int? familyGroupId)
    {
        return Ok(await _mediator.Send(new GenerateShoppingListQuery(familyGroupId)));
    }
}
