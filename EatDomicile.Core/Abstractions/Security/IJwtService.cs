using Microsoft.AspNetCore.Identity;

namespace EatDomicile.Core.Abstractions.Security;

public interface IJwtService
{
    string GenerateToken(IdentityUser user);
}
