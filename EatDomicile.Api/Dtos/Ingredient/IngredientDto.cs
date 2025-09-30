using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Api.Dtos.Ingredient
{
    public class IngredientDto
    {
        public decimal KCal { get; set; }

        public string Name { get; set; }

        public bool IsAllergen { get; set; }
    }
}
