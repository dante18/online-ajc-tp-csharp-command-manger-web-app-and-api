namespace EatDomicile.Web.Services.Services.Security;

using EatDomicile.Web.Services.Abstractions.Security;
using EatDomicile.Web.Services.Dtos.Security;
using System.Text;
using System.Text.Json;

internal class AuthService : IAuthService
{
    private readonly HttpClient httpClient;

    public AuthService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("Auth");
    }

    public async Task<string?> LoginAsync(LoginDto model)
    {
        var loginModel = new
        {
            model.Email,
            model.Password,
        };

        var json = JsonSerializer.Serialize(loginModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync("login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponseDto>(responseContent,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });

                return authResponse?.Token;
            }
        }
        catch (Exception)
        {
            // Log exception
        }

        return null;
    }

    public async Task<bool> RegisterAsync(RegisterDto model)
    {
        var registerModel = new
        {
            model.Email,
            model.Password,
            model.ConfirmPassword,
        };

        var json = JsonSerializer.Serialize(registerModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync("register", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            // Log exception
            return false;
        }
    }
}
