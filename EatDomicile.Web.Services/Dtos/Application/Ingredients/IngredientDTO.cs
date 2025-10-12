namespace EatDomicile.Web.Services.Dtos.Application.Ingredients;

public class IngredientDTO
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal KCal { get; set; }

    public bool IsAllergen { get; set; }
}
