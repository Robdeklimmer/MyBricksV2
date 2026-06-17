using MyBricks.Domain.Entities;

namespace MyBricks.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);
}
