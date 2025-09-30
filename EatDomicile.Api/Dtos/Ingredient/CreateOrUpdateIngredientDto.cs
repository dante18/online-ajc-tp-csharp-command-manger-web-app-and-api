using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Ingredient
{
    public class CreateOrUpdateIngredientDto
    {
        [Required]
        public decimal KCal { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsAllergen { get; set; }
    }
}
