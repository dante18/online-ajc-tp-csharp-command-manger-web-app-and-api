using EatDomicile.Api.Dtos.Address;
using EatDomicile.Api.Dtos.Common;
using EatDomicile.Api.Dtos.User;
using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> logger;

    private readonly IUserService userService;

    private readonly UserManager<IdentityUser> userManager;

    private readonly RoleManager<IdentityRole> roleManager;

    public UsersController(ILogger<UsersController> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IUserService userService)
    {
        this.logger = logger;
        this.userService = userService;
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.logger.LogInformation("UsersController initialized");
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        this.logger.LogInformation("GetUsers called");

        try
        {
            var queryResult = this.userService.GetAllUsers();
            var queryResultWithPagination = await queryResult.ToPagedResultAsync(pageNumber, pageSize);
            List<UserDto> users = new List<UserDto>();

            foreach (var u in queryResultWithPagination.Items)
            {
                IdentityUser? user = await this.userManager.FindByEmailAsync(u.Mail);
                string role = "";

                if (user is not null)
                {
                    var roleIdentity = await userManager.GetRolesAsync(user);
                    role = roleIdentity.FirstOrDefault();
                }

                users.Add(new UserDto()
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Phone = u.Phone,
                    Mail = u.Mail,
                    Role = role,
                    Address = new AddressDto()
                    {
                        Id = u.Address.Id,
                        Street = u.Address.Street,
                        City = u.Address.City,
                        State = u.Address.State,
                        Zip = u.Address.Zip,
                        Country = u.Address.Country
                    }
                });
            }

            PaginatedResponse<UserDto> response = new PaginatedResponse<UserDto>()
            {
                Items = users,
                currentPage = pageNumber,
                maxPage = queryResultWithPagination.TotalPages,
                nextPage = pageNumber == queryResultWithPagination.TotalPages
                    ? queryResultWithPagination.TotalPages
                    : (pageNumber + 1),
                previousPage = pageNumber == 1 ? 1 : (pageNumber - 1)
            };

            this.logger.LogInformation("GetUsers completed successfully. Found {UserCount} users", users.Count);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching users");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> GetUserAsync([FromRoute] int id)
    {
        this.logger.LogInformation("GetUser called with ID: {UserId}", id);

        try
        {
            User user = this.userService.GetUser(id);
            if (user is null)
            {
                this.logger.LogWarning("User not found with ID: {UserId}", id);
                return Results.NotFound($"User not found by id : {id}");
            }

            this.logger.LogInformation("GetUser completed successfully for ID: {UserId}, Name: {UserName}",
                id,
                user.FirstName + " " + user.LastName);

            IdentityUser? userIdentity = await this.userManager.FindByEmailAsync(user.Mail);
            string role = "";

            if (userIdentity is not null)
            {
                var roleIdentity = await userManager.GetRolesAsync(userIdentity);
                role = roleIdentity.FirstOrDefault();
            }

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Mail = user.Mail,
                Role = role,
                Address = new AddressDto()
                {
                    Id = user.Address.Id,
                    Street = user.Address.Street,
                    City = user.Address.City,
                    State = user.Address.State,
                    Zip = user.Address.Zip,
                    Country = user.Address.Country
                }
            };

            return Results.Ok(userDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching user with ID: {UserId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> CreateUser([FromBody] CreateUserDto dto)
    {
        this.logger.LogInformation("CreateUser called with FirstName: {FirstName}, LastName: {LastName}, Phone: {Phone}, Mail: {Mail}",
            dto.FirstName,
            dto.LastName,
            dto.Phone,
            dto.Mail);

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            User user = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Mail = dto.Mail,
                Address = new Address()
                {
                    Street = dto.Address.Street,
                    City = dto.Address.City,
                    State = dto.Address.State,
                    Zip = dto.Address.Zip,
                    Country = dto.Address.Country
                }
            };

            this.logger.LogDebug("Adding new user to context: {UserDetails}",
                new
                {
                    dto.FirstName,
                    dto.LastName,
                    dto.Phone,
                    dto.Mail
                }
            );

            this.userService.CreateUser(user);

            var userExists = await this.userManager.FindByEmailAsync(dto.Mail);
            if (userExists is null)
            {
                var userFound = new IdentityUser
                {
                    Email = dto.Mail,
                    UserName = dto.Mail,
                };

                var result = await this.userManager.CreateAsync(userFound, dto.Password);

                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(dto.Role))
                        await roleManager.CreateAsync(new IdentityRole(dto.Role));

                    await userManager.AddToRoleAsync(userFound, dto.Role);
                }
            }

            this.logger.LogInformation("CreateUser completed successfully. New user ID: {UserId}, Name: {Name}",
                user.Id,
                user.FirstName + " " + user.LastName);

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Mail = user.Mail,
                Address = new AddressDto()
                {
                    Id = user.Address.Id,
                    Street = user.Address.Street,
                    City = user.Address.City,
                    State = user.Address.State,
                    Zip = user.Address.Zip,
                    Country = user.Address.Country
                }
            };

            return Results.Created($"/api/users/{user.Id}", userDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e,
                "An error occurred while creating user with Name: {Name}",
                dto.FirstName + " " + dto.LastName);

            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> UpdateUserAsync([FromRoute] int id, [FromBody] UpdateUserDto dto)
    {
        this.logger.LogInformation("UpdateUser called for ID: {UserId} with data: {UpdateData}",
            id,
            new
            {
                dto.FirstName,
                dto.LastName,
                dto.Phone,
                dto.Mail
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            User user = this.userService.GetUser(id);
            if (user is null)
            {
                this.logger.LogWarning("Cannot update user - not found with ID: {UserId}", id);
                return Results.NotFound($"User not found by id : {id}");
            }

            this.logger.LogDebug("Found existing user: {UserName}",
                user.FirstName + " " + user.LastName);

            if (dto.FirstName != null) user.FirstName = dto.FirstName;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.Phone != null) user.Phone = dto.Phone;
            if (dto.Mail != null) user.Mail = dto.Mail;
            if (dto.Address.Street != null) user.Address.Street = dto.Address.Street;
            if (dto.Address.City != null) user.Address.City = dto.Address.City;
            if (dto.Address.State != null) user.Address.State = dto.Address.State;
            if (dto.Address.Zip != null) user.Address.Zip = dto.Address.Zip;
            if (dto.Address.Country != null) user.Address.Country = dto.Address.Country;

            this.userService.UpdateUser(user);

            var userExists = await this.userManager.FindByEmailAsync(dto.Mail);
            if (userExists is not null)
            {
                if (!await roleManager.RoleExistsAsync(dto.Role))
                {
                    await roleManager.CreateAsync(new IdentityRole(dto.Role));
                }

                var currentRoles = await userManager.GetRolesAsync(userExists);
                if (currentRoles.Any())
                {
                    await userManager.RemoveFromRolesAsync(userExists, currentRoles);
                }

                await userManager.AddToRoleAsync(userExists, dto.Role);
            }

            this.logger.LogInformation("UpdateUser completed successfully for ID: {UserId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating user with ID: {UserId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> DeleteUser(int id)
    {
        this.logger.LogInformation("DeleteUser called for ID: {UserId}", id);

        try
        {
            User user = this.userService.GetUser(id);
            if (user is null)
            {
                this.logger.LogWarning("Cannot delete user - not found with ID: {UserId}", id);
                return Results.NotFound($"User not found by id : {id}");
            }

            this.logger.LogInformation("Deleting user: ID={UserId}, Name='{UserName}'",
                user.Id,
                user.FirstName + " " + user.LastName);

            this.userService.DeleteUser(user);

            this.logger.LogInformation("DeleteUser completed successfully for ID: {UserId}", id);
            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting user with ID: {UserId}", id);
            return Results.InternalServerError(e);
        }
    }
}
