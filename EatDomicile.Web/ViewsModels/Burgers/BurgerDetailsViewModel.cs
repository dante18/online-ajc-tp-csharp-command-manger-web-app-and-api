using EatDomicile.Web.Services.Dtos.Application.Burgers;
using EatDomicile.Web.Services.Dtos.Application.Ingredients;

namespace EatDomicile.Web.ViewsModels.Burgers;

public class BurgerDetailsViewModel
{
    public BurgerDTO Burger { get; set; }

    public IEnumerable<IngredientDTO> Ingredients { get; set; }
}
