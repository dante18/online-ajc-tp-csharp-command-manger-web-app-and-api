namespace EatDomicile.Web.Services.Domains.Ingredients.DTO;

public class IngredientDTO
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal KCal { get; set; }

    public bool IsAllergen { get; set; }
}
