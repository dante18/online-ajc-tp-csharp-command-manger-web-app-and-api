namespace EatDomicile.Web.Services.Dtos.Security;

public sealed record RegisterDto
{
    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string ConfirmPassword { get; init; } = string.Empty;
}
