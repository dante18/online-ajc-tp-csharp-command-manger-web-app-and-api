using EatDomicile.Web.Services.Dtos.Application.Ingredients;
using EatDomicile.Web.Services.Dtos.Application.Pizzas;

namespace EatDomicile.Web.ViewsModels.Pizzas;

public class PizzaDetailsViewModel
{
    public PizzaDTO Pizza { get; set; }

    public IEnumerable<IngredientDTO> Ingredients { get; set; }
}
