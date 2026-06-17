using FluentValidation;
using MediatR;
using MyBricks.Application.Common.Exceptions;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.FamilyGroups.DTOs;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Exceptions;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Application.Features.FamilyGroups.Commands;

public record JoinFamilyGroupCommand(string InviteCode) : IRequest<FamilyGroupDto>;

public class JoinFamilyGroupCommandValidator : AbstractValidator<JoinFamilyGroupCommand>
{
    public JoinFamilyGroupCommandValidator()
    {
        RuleFor(x => x.InviteCode).NotEmpty();
    }
}

public class JoinFamilyGroupCommandHandler : IRequestHandler<JoinFamilyGroupCommand, FamilyGroupDto>
{
    private readonly IFamilyGroupRepository _groupRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public JoinFamilyGroupCommandHandler(
        IFamilyGroupRepository groupRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<FamilyGroupDto> Handle(JoinFamilyGroupCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var group = await _groupRepository.GetByInviteCodeAsync(request.InviteCode, cancellationToken);

        if (group == null)
            throw new NotFoundException(nameof(FamilyGroup), request.InviteCode);

        // Check if already a member
        if (group.UserFamilyGroups.Any(ug => ug.UserId == userId))
            throw new DomainException("You are already a member of this group.");

        group.UserFamilyGroups.Add(new UserFamilyGroup
        {
            UserId = userId,
            FamilyGroupId = group.Id
        });

        await _groupRepository.UpdateAsync(group, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FamilyGroupDto
        {
            Id = group.Id,
            Name = group.Name,
            InviteCode = group.InviteCode,
            OwnerUserId = group.OwnerUserId,
            CreatedAt = group.CreatedAt
        };
    }
}
