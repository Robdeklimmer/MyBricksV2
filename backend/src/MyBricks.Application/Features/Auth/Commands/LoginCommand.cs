using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyBricks.Application.Common.Exceptions;
using MyBricks.Application.Common.Interfaces;
using MyBricks.Application.Features.Auth.DTOs;
using MyBricks.Domain.Entities;

namespace MyBricks.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        var result = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
            throw new UnauthorizedAccessException("Invalid email or password.");

        return new AuthResponse
        {
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName,
            Email = user.Email
        };
    }
}
