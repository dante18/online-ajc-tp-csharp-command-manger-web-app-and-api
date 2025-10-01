using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Order
{
    public class CreateOrderProductDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
