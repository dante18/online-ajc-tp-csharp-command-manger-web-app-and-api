using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Order
{
    public class CreateOrUpdateOrderDto
    {
        [Required]
        public int? Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
