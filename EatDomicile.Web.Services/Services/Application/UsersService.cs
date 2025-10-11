using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Users;
using EatDomicile.Web.Services.Dtos.Common;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Services.Application;

internal class UsersService : IApiUsersService
{
    private readonly HttpClient httpClient;

    public UsersService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Users");
    }

    public async Task<PaginatedResponseDto<UserDTO>> GetUsersAsync(int page = 1)
    {
        var users = await httpClient.GetFromJsonAsync<PaginatedResponseDto<UserDTO>>($"?pageNumber={page}");
        return users ?? new PaginatedResponseDto<UserDTO>()
        {
            Items = new List<UserDTO>(),
            currentPage = 1,
            maxPage = 1,
            nextPage = 1,
            previousPage = 1
        };
    }

    public async Task<UserDTO?> GetUserAsync(int id)
    {
        var user = await httpClient.GetFromJsonAsync<UserDTO>($"{id}");
        return user;
    }

    public async Task CreateUserAsync(CreateUserDTO userDTO)
    {
        var response = await httpClient.PostAsJsonAsync("", userDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateUserAsync(int id, UserDTO userDto)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}", userDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
