using MediatR;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.FamilyGroups.DTOs;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Application.Features.FamilyGroups.Queries;

public record GetMyFamilyGroupsQuery : IRequest<IReadOnlyList<FamilyGroupDto>>;

public class GetMyFamilyGroupsQueryHandler : IRequestHandler<GetMyFamilyGroupsQuery, IReadOnlyList<FamilyGroupDto>>
{
    private readonly IFamilyGroupRepository _groupRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetMyFamilyGroupsQueryHandler(
        IFamilyGroupRepository groupRepository,
        ICurrentUserService currentUserService)
    {
        _groupRepository = groupRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<FamilyGroupDto>> Handle(GetMyFamilyGroupsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        // This method will need to be added to IFamilyGroupRepository
        var groups = await _groupRepository.GetByUserIdAsync(userId, cancellationToken);

        return groups.Select(g => new FamilyGroupDto
        {
            Id = g.Id,
            Name = g.Name,
            InviteCode = g.InviteCode,
            OwnerUserId = g.OwnerUserId,
            CreatedAt = g.CreatedAt
        }).ToList();
    }
}
