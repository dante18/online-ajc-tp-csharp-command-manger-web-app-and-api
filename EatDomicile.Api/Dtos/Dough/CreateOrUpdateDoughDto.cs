using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Dough
{
    public class CreateOrUpdateDoughDto
    {
        [Required]
        public string Name { get; set; }
    }
}
