using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Pizza;

public class CreateOrUpdatePizzaDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Range(1.50, 20)]
    public decimal Price { get; set; }

    [Required]
    public int DoughsId { get; set; }

    [Required]
    public bool Vegetarian { get; set; }
}
