using System.Net.Http.Json;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.Services.Users.DTO;

namespace EatDomicile.Web.Services.Users;

public class UsersService : IApiUsersService
{
    private readonly HttpClient httpClient;

    public UsersService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<IEnumerable<UserDTO>> GetUsersAsync()
    {
        var users = await httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("https://localhost:7001/api/users");
        return users ?? [];
    }

    public async Task<UserDTO?> GetUserAsync(int id)
    {
        var user = await httpClient.GetFromJsonAsync<UserDTO>($"https://localhost:7001/api/users/{id}");
        return user;
    }

    public async Task CreateUserAsync(UserDTO userDTO)
    {
        var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/users", userDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateUserAsync(int id, UserDTO userDto)
    {
        var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/users/{id}", userDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/users/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}