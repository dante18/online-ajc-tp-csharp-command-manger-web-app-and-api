using EatDomicile.Web.Services.Dtos.Security;

namespace EatDomicile.Web.Services.Abstractions.Security;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginDto model);

    Task<bool> RegisterAsync(RegisterDto model);
}
