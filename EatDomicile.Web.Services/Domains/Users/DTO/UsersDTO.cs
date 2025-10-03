using System.ComponentModel;
using EatDomicile.Web.Services.Domains.Addresses.DTO;

namespace EatDomicile.Web.Services.Users.DTO;

public class UsersDTO
{
    public int? Id { get; set; }

    [DisplayName("FirstName")]
    public string FirstName { get; set; }

    [DisplayName("LastName")]
    public string LastName { get; set; }

    [DisplayName("Phone")]
    public string Phone { get; set; }

    [DisplayName("Mail")]
    public string Mail { get; set; }

    [DisplayName("Adresse")]
    public AddressDTO Address { get; set; }
}
