using FluentValidation;
using MediatR;
using MyBricks.Application.Common.Exceptions;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.Parts.DTOs;
using MyBricks.Domain.Entities;
using MyBricks.Domain.Enums;
using MyBricks.Domain.Exceptions;
using MyBricks.Domain.Interfaces;

namespace MyBricks.Application.Features.Parts.Commands;

public record FlagMissingPartCommand(
    int UserSetId, 
    int PartId, 
    int QuantityMissing, 
    PartCondition Condition, 
    string? Note) : IRequest<MissingPartDto>;

public class FlagMissingPartCommandValidator : AbstractValidator<FlagMissingPartCommand>
{
    public FlagMissingPartCommandValidator()
    {
        RuleFor(x => x.QuantityMissing).GreaterThan(0);
        RuleFor(x => x.Condition).IsInEnum();
    }
}

public class FlagMissingPartCommandHandler : IRequestHandler<FlagMissingPartCommand, MissingPartDto>
{
    private readonly IUserSetRepository _userSetRepository;
    private readonly IMissingPartRepository _missingPartRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;

    public FlagMissingPartCommandHandler(
        IUserSetRepository userSetRepository,
        IMissingPartRepository missingPartRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork)
    {
        _userSetRepository = userSetRepository;
        _missingPartRepository = missingPartRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<MissingPartDto> Handle(FlagMissingPartCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

        var userSet = await _userSetRepository.GetByIdAsync(request.UserSetId, cancellationToken);
        if (userSet == null)
            throw new NotFoundException(nameof(UserSet), request.UserSetId);

        // Security check: must own the set, or be in the family group that owns it.
        // For simplicity, we assume the repository method GetByIdAsync includes the FamilyGroup.UserFamilyGroups
        // Or we can just let a higher-level authorization behavior handle this.
        // We'll enforce a simple check here:
        bool isOwner = userSet.UserId == userId;
        bool isGroupMember = userSet.FamilyGroup?.UserFamilyGroups.Any(ug => ug.UserId == userId) ?? false;

        if (!isOwner && !isGroupMember)
            throw new UnauthorizedAccessException("You do not have access to this set.");

        var missingPart = new MissingPart
        {
            UserSetId = request.UserSetId,
            PartId = request.PartId,
            QuantityMissing = request.QuantityMissing,
            Condition = request.Condition,
            Note = request.Note,
            FlaggedAt = DateTime.UtcNow
        };

        // Mark the set as incomplete
        userSet.IsComplete = false;

        await _missingPartRepository.AddAsync(missingPart, cancellationToken);
        await _userSetRepository.UpdateAsync(userSet, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Normally we'd return the DTO mapped from the entity (with part details).
        // The repository should load the Part navigation property upon Add or we re-fetch.
        return new MissingPartDto
        {
            Id = missingPart.Id,
            UserSetId = missingPart.UserSetId,
            PartId = missingPart.PartId,
            QuantityMissing = missingPart.QuantityMissing,
            Condition = missingPart.Condition,
            Note = missingPart.Note,
            FlaggedAt = missingPart.FlaggedAt,
            IsResolved = missingPart.IsResolved
        };
    }
}
