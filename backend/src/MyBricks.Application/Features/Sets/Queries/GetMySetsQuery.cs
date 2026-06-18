using MediatR;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.Sets.DTOs;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Application.Features.Sets.Queries;

public record GetMySetsQuery(int? FamilyGroupId) : IRequest<IReadOnlyList<UserSetDto>>;

public class GetMySetsQueryHandler : IRequestHandler<GetMySetsQuery, IReadOnlyList<UserSetDto>>
{
    private readonly IUserSetRepository _userSetRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMySetsQueryHandler(
        IUserSetRepository userSetRepository,
        ICurrentUserService currentUserService)
    {
        _userSetRepository = userSetRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<UserSetDto>> Handle(GetMySetsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var sets = request.FamilyGroupId.HasValue
            ? await _userSetRepository.GetByGroupIdAsync(request.FamilyGroupId.Value, cancellationToken)
            : await _userSetRepository.GetByUserIdAsync(userId, cancellationToken); // Assuming this filters for FamilyGroupId == null in the impl

        return sets.Select(s => new UserSetDto
        {
            Id = s.Id,
            LegoSet = new LegoSetDto
            {
                Id = s.LegoSet.Id,
                RebrickableSetNum = s.LegoSet.RebrickableSetNum,
                Name = s.LegoSet.Name,
                Year = s.LegoSet.Year,
                Theme = s.LegoSet.Theme,
                TotalParts = s.LegoSet.TotalParts,
                ImageUrl = s.LegoSet.ImageUrl,
                LastSyncedAt = s.LegoSet.LastSyncedAt
            },
            FamilyGroupId = s.FamilyGroupId,
            IsComplete = s.IsComplete,
            AddedAt = s.AddedAt
        }).ToList();
    }
}
