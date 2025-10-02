using EatDomicile.Web.Services.Addresses.DTO;
using EatDomicile.Web.Services.Users.DTO;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Users
{
    public class UsersDetailsViewModel
    {
        public UserDTO User { get; set; }

        public IEnumerable<AddressDTO> Addresses { get; set; }
    }
}
