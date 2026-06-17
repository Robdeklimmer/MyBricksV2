using MediatR;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.ShoppingList.DTOs;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Application.Features.ShoppingList.Queries;

/// <summary>
/// If FamilyGroupId is null, gets personal missing parts. 
/// If FamilyGroupId has value, gets aggregated missing parts for the group.
/// </summary>
public record GenerateShoppingListQuery(int? FamilyGroupId) : IRequest<IReadOnlyList<ShoppingListItemDto>>;

public class GenerateShoppingListQueryHandler : IRequestHandler<GenerateShoppingListQuery, IReadOnlyList<ShoppingListItemDto>>
{
    private readonly IMissingPartRepository _missingPartRepository;
    private readonly ICurrentUserService _currentUserService;

    public GenerateShoppingListQueryHandler(
        IMissingPartRepository missingPartRepository,
        ICurrentUserService currentUserService)
    {
        _missingPartRepository = missingPartRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<ShoppingListItemDto>> Handle(GenerateShoppingListQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var missingParts = request.FamilyGroupId.HasValue
            ? await _missingPartRepository.GetUnresolvedByGroupIdAsync(request.FamilyGroupId.Value, cancellationToken)
            : await _missingPartRepository.GetUnresolvedPersonalAsync(userId, cancellationToken);

        // Aggregate by PartId
        var aggregated = missingParts
            .GroupBy(mp => mp.PartId)
            .Select(g =>
            {
                var first = g.First();
                return new ShoppingListItemDto
                {
                    PartId = g.Key,
                    RebrickablePartNum = first.Part.RebrickablePartNum,
                    Name = first.Part.Name,
                    Color = first.Part.Color,
                    ImageUrl = first.Part.ImageUrl,
                    TotalQuantityNeeded = g.Sum(x => x.QuantityMissing),
                    EstimatedPricePerUnit = first.Part.PriceCache?.AveragePriceEur ?? 0m
                };
            })
            .ToList();

        return aggregated;
    }
}
