using EatDomicile.Web.Services.Domains.Burgers.DTO;
using EatDomicile.Web.Services.Domains.Ingredients.DTO;

namespace EatDomicile.Web.ViewModels.Burgers;

public class BurgerDetailsViewModel
{
    public BurgerDTO Burger { get; set; }

    public IEnumerable<IngredientDTO> Ingredients { get; set; }
}
