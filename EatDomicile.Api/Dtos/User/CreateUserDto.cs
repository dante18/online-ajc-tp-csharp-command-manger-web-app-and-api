using EatDomicile.Api.Dtos.Address;
using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.User;

public class CreateUserDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    public string Mail { get; set; }

    [Required]
    public AddressDto Address { get; set; }

    [Required]
    public string Role { get; set; }

    [Required]
    public string Password { get; set; }
}
