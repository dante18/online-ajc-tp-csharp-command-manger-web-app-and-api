using EatDomicile.Api.Dtos.Address;
using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.User;

public class CreateOrUpdateUserDto
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
}
