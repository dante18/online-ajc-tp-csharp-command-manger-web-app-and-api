using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Doughs;

public class CreateOrUpdateDoughsDto
{
    [Required]
    public string Name { get; set; }
}
