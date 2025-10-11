using System.ComponentModel;
using EatDomicile.Web.Services.Dtos.Application.Addresses;

namespace EatDomicile.Web.Services.Dtos.Application.Users;

public class CreateUserDTO
{
    public int? Id { get; set; }

    [DisplayName("Prénom")]
    public string FirstName { get; set; }

    [DisplayName("Nom")]
    public string LastName { get; set; }

    [DisplayName("Téléphone")]
    public string Phone { get; set; }

    [DisplayName("Email")]
    public string Mail { get; set; }

    [DisplayName("Adresse")]
    public AddressDTO Address { get; set; }

    [DisplayName("Mot de passe")]
    public string Password { get; set; }

    [DisplayName("Rôle")]
    public string Role { get; set; }

}
