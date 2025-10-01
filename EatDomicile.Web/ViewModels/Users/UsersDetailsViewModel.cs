using EatDomicile.Web.Services.Addresses.DTO;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Users
{
    public class UsersDetailsViewModel
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
}
