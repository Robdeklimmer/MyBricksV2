using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyBricks.Application.Common.Exceptions;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.Auth.DTOs;
using MyBricks.Domain.Entities;
using ValidationException = MyBricks.Application.Common.Exceptions.ValidationException;

namespace MyBricks.Application.Features.Auth.Commands;

public record RegisterCommand(string Email, string Password, string DisplayName) : IRequest<AuthResponse>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(50);
    }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            DisplayName = request.DisplayName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToArray();
            throw new ValidationException(new Dictionary<string, string[]> { { "Identity", errors } });
        }

        return new AuthResponse
        {
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName,
            Email = user.Email
        };
    }
}
