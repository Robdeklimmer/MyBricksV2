using FluentValidation;
using MediatR;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.FamilyGroups.DTOs;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Application.Features.FamilyGroups.Commands;

public record CreateFamilyGroupCommand(string Name) : IRequest<FamilyGroupDto>;

public class CreateFamilyGroupCommandValidator : AbstractValidator<CreateFamilyGroupCommand>
{
    public CreateFamilyGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

public class CreateFamilyGroupCommandHandler : IRequestHandler<CreateFamilyGroupCommand, FamilyGroupDto>
{
    private readonly IFamilyGroupRepository _groupRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFamilyGroupCommandHandler(
        IFamilyGroupRepository groupRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<FamilyGroupDto> Handle(CreateFamilyGroupCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var group = new FamilyGroup
        {
            Name = request.Name,
            OwnerUserId = userId,
            InviteCode = GenerateInviteCode(),
        };

        // Automatically add the creator as a member
        group.UserFamilyGroups.Add(new UserFamilyGroup
        {
            UserId = userId
        });

        await _groupRepository.AddAsync(group, cancellationToken);
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

    private static string GenerateInviteCode()
    {
        // Simple 6-character alphanumeric code
        return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
    }
}
