namespace EatDomicile.Web.Services.Dtos.Security;

public sealed record AuthResponseDto
{
    public string Token { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public DateTime Expiration { get; init; }
}
