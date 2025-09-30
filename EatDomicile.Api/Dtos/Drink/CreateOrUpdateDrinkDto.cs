using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Drink;

public class CreateOrUpdateDrinkDto
{
    [Required]
    public decimal KCal { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [Range(1.50, 20)]
    public decimal Price { get; set; }

    [Required]
    public bool Fizzy { get; set; }
}
