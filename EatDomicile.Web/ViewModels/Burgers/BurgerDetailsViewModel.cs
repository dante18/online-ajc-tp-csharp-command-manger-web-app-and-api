using EatDomicile.Web.Services.Burgers.DTO;
using EatDomicile.Web.Services.Ingredient.DTO;

namespace EatDomicile.Web.ViewModels.Burgers;

public class BurgerDetailsViewModel
{
    public BurgerDTO Burger { get; set; }

    public IEnumerable<IngredientDTO> Ingredients { get; set; }
}
