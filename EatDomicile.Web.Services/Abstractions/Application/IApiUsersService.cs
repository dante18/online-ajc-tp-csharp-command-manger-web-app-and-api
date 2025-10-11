using EatDomicile.Web.Services.Dtos.Application.Users;
using EatDomicile.Web.Services.Dtos.Common;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiUsersService
{
    public Task<PaginatedResponseDto<UserDTO>> GetUsersAsync(int page = 1);

    public Task<UserDTO?> GetUserAsync(int id);

    public Task CreateUserAsync(CreateUserDTO userDTO);

    public Task UpdateUserAsync(int id, UserDTO userDto);

    public Task DeleteUserAsync(int id);
}