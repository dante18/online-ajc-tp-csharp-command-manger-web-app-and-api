using EatDomicile.Web.Services.Addresses;
using EatDomicile.Web.Services.Addresses.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatDomicile.Web.Services.Users.DTO
{
    public class UserDTO
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
