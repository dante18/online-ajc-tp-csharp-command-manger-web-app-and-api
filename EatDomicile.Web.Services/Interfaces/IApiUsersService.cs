using EatDomicile.Web.Services.Domains.Users.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiUsersService
{
    public Task<IEnumerable<UserDTO>> GetUsersAsync();

    public Task<UserDTO?> GetUserAsync(int id);

    public Task CreateUserAsync(UserDTO userDTO);

    public Task UpdateUserAsync(int id, UserDTO userDto);

    public Task DeleteUserAsync(int id);
}