using EatDomicile.Api.Dtos.Address;
using EatDomicile.Api.Dtos.Pasta;
using EatDomicile.Api.Dtos.User;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using System.IO;

namespace EatDomicile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersControllers : ControllerBase
    {
        private readonly UserService userService;

        public UsersControllers(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public IResult GetUser()
        {
            List<UserDto> users = this.userService.GetAllUsers().Select(u => new UserDto()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Phone = u.Phone,
                Mail = u.Mail,
                Address = new AddressDto()
                {
                    Id = u.Address.Id,
                    Street = u.Address.Street,
                    City = u.Address.City,
                    State = u.Address.State,
                    Zip = u.Address.Zip,
                    Country = u.Address.Country
                }
            }).ToList();

            return Results.Ok(users);
        }

        [HttpGet("{id}")]
        public IResult GetUser([FromRoute] int id)
        {
            User user = this.userService.GetUser(id);
            if (user is null)
                return Results.NotFound($"User not found by id : {id}");

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Mail = user.Mail,
                Address =
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

        [HttpPost()]
        public IResult CreateUser([FromBody] CreateOrUpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

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

            this.userService.CreateUser(user);

            UserDto userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Mail = user.Mail,
                Address =
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

        [HttpPut("{id}")]
        public IResult UpdateUser([FromRoute] int id, [FromBody] CreateOrUpdateUserDto dto)
        {
            User user = this.userService.GetUser(id);
            if (user is null)
                return Results.NotFound($"User not found by id : {id}");

            if (dto.FirstName != null) user.FirstName = dto.FirstName;
            if (dto.LastName != null) user.LastName = dto.LastName;
            if (dto.Phone != null) user.Phone = dto.Phone;
            if (dto.Mail != null) user.Mail = dto.Mail;
            if (dto.Address != null) user.Address = new Address()
            {
                Id = user.Address.Id,
                Street = user.Address.Street,
                City = user.Address.City,
                State = user.Address.State,
                Zip = user.Address.Zip,
                Country = user.Address.Country
            };

            this.userService.UpdateUser(user);

            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteUser(int id)
        {
            User user = this.userService.GetUser(id);
            if (user is null)
                return Results.NotFound($"User not found by id : {id}");

            this.userService.DeleteUser(user);

            return Results.NoContent();
        }
    }
}
