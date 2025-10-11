namespace EatDomicile.Web.Services.Dtos.Security;

public sealed record LoginDto
{
    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public bool RememberMe { get; init; }
}
