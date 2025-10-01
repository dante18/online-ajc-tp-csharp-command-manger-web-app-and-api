using EatDomicile.Api.Dtos.Address;

namespace EatDomicile.Api.Dtos.User
{
    public class UserDto
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Mail { get; set; }

        public AddressDto Address { get; set; }
    }
}
