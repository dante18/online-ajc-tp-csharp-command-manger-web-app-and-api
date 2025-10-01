using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Address
{
    public class CreateOrUpdateAddressDto
    {
        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
