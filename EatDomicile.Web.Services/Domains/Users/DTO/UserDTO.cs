using System.ComponentModel;
using EatDomicile.Web.Services.Domains.Addresses.DTO;

namespace EatDomicile.Web.Services.Domains.Users.DTO;

public class UserDTO
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

}
