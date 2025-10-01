using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Order;

public class CreateOrUpdateOrderDto
{
    [Required]
    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    [Required]
    public int Status { get; set; }

    [Required]
    public int UserId { get; set; }
}
