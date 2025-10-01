using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Pasta;

public class CreateOrUpdatePastaDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    [Range(8.50, 20)]
    public decimal Price { get; set; }

    [Required]
    public int Type { get; set; }

    [Required]
    public decimal KCal { get; set; }

    [Required]
    public bool Vegetarian { get; set; }
}
