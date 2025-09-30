using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Burger
{
    public class CreateOrUpdateBurgerDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1.50, 20)]
        public decimal Price { get; set; }
    }
}
