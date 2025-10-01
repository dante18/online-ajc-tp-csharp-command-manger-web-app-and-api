using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Dough;

public class CreateOrUpdateDoughsDto
{
    [Required]
    public string Name { get; set; }
}
