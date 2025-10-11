using EatDomicile.Web.Services.Dtos.Application.Addresses;
using EatDomicile.Web.Services.Dtos.Application.Users;

namespace EatDomicile.Web.ViewsModels.Users;

public class UsersDetailsViewModel
{
    public UserDTO User { get; set; }

    public AddressDTO Address { get; set; }
}
