using EatDomicile.Web.Services.Domains.Ingredients.DTO;
using EatDomicile.Web.Services.Domains.Pizzas.DTO;

namespace EatDomicile.Web.ViewModels.Pizzas;

public class PizzaDetailsViewModel
{
    public PizzaDTO Pizza { get; set; }

    public IEnumerable<IngredientDTO> Ingredients { get; set; }
}
