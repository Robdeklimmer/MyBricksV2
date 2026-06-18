using FluentValidation;
using MediatR;
using MyBricks.Application.Common.Exceptions;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.Sets.DTOs;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Exceptions;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Application.Features.Sets.Commands;

public record AddSetCommand(string RebrickableSetNum, int? FamilyGroupId) : IRequest<UserSetDto>;

public class AddSetCommandValidator : AbstractValidator<AddSetCommand>
{
    public AddSetCommandValidator()
    {
        RuleFor(x => x.RebrickableSetNum).NotEmpty();
    }
}

public class AddSetCommandHandler : IRequestHandler<AddSetCommand, UserSetDto>
{
    private readonly ILegoSetRepository _legoSetRepository;
    private readonly IUserSetRepository _userSetRepository;
    private readonly IFamilyGroupRepository _groupRepository;
    private readonly IRebrickableClient _rebrickableClient;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public AddSetCommandHandler(
        ILegoSetRepository legoSetRepository,
        IUserSetRepository userSetRepository,
        IFamilyGroupRepository groupRepository,
        IRebrickableClient rebrickableClient,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _legoSetRepository = legoSetRepository;
        _userSetRepository = userSetRepository;
        _groupRepository = groupRepository;
        _rebrickableClient = rebrickableClient;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserSetDto> Handle(AddSetCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        // If FamilyGroupId is provided, ensure user is a member
        if (request.FamilyGroupId.HasValue)
        {
            var group = await _groupRepository.GetByIdAsync(request.FamilyGroupId.Value, cancellationToken);
            if (group == null)
                throw new NotFoundException(nameof(FamilyGroup), request.FamilyGroupId.Value);

            if (!group.UserFamilyGroups.Any(ug => ug.UserId == userId))
                throw new DomainException("You must be a member of the family group to add a set to it.");
        }

        var setNum = request.RebrickableSetNum;
        if (!setNum.Contains("-"))
        {
            setNum += "-1";
        }

        // 1. Get or create the Canonical LegoSet
        var legoSet = await _legoSetRepository.GetByRebrickableNumAsync(setNum, cancellationToken);

        if (legoSet == null)
        {
            // Fetch from Rebrickable
            var fetchedSet = await _rebrickableClient.GetSetDetailsAsync(setNum, cancellationToken);
            if (fetchedSet == null)
                throw new NotFoundException("RebrickableSet", setNum);

            // Fetch parts
            var parts = await _rebrickableClient.GetSetPartsAsync(setNum, cancellationToken);
            foreach (var setPart in parts)
            {
                fetchedSet.SetParts.Add(setPart);
            }

            legoSet = await _legoSetRepository.AddAsync(fetchedSet, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken); // Save canonical set first
        }

        // 2. Create the UserSet
        var userSet = new UserSet
        {
            UserId = userId,
            LegoSetId = legoSet.Id,
            FamilyGroupId = request.FamilyGroupId,
            IsComplete = true // Assume complete until parts are flagged missing
        };

        await _userSetRepository.AddAsync(userSet, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserSetDto
        {
            Id = userSet.Id,
            LegoSet = new LegoSetDto
            {
                Id = legoSet.Id,
                RebrickableSetNum = legoSet.RebrickableSetNum,
                Name = legoSet.Name,
                Year = legoSet.Year,
                Theme = legoSet.Theme,
                TotalParts = legoSet.TotalParts,
                ImageUrl = legoSet.ImageUrl,
                LastSyncedAt = legoSet.LastSyncedAt
            },
            FamilyGroupId = userSet.FamilyGroupId,
            IsComplete = userSet.IsComplete,
            AddedAt = userSet.AddedAt
        };
    }
}
