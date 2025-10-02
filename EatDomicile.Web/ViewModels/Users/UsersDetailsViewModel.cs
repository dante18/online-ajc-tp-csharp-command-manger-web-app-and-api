using EatDomicile.Web.Services.Domains.Addresses.DTO;
using EatDomicile.Web.Services.Domains.Users.DTO;

namespace EatDomicile.Web.ViewModels.Users;

public class UsersDetailsViewModel
{
    public UserDTO User { get; set; }

    public AddressDTO Address { get; set; }
}
