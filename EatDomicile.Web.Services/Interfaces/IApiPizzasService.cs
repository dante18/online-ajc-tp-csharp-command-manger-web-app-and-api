using EatDomicile.Web.Services.Domains.Ingredients.DTO;
using EatDomicile.Web.Services.Domains.Pizzas.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiPizzasService
{
    public Task<IEnumerable<PizzaDTO>> GetPizzasAsync();

    public Task<PizzaDTO?> GetPizzaAsync(int id);

    public Task<IEnumerable<IngredientDTO>> GetPizzaIngredientsAsync(int id);

    public Task CreatePizzaAsync(CreatePizzaDTO pizzaDTO);

    public Task UpdatePizzaAsync(int id, CreatePizzaDTO pizzaDTO);

    public Task UpdatePizzaAddIngredientAsync(int id, IngredientDTO ingredientDTO);

    public Task UpdatePizzaUpdateIngredientAsync(int id, int ingredientId, IngredientDTO ingredientDTO);

    public Task UpdatePizzaDeleteIngredientAsync(int id, int ingredientId);

    public Task DeletePizzaAsync(int id);
}