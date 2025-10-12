using EatDomicile.Web.Services.Dtos.Application.Doughs;
using EatDomicile.Web.Services.Dtos.Application.Ingredients;
using EatDomicile.Web.Services.Dtos.Application.Pizzas;
using EatDomicile.Web.Services.Dtos.Common;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiPizzasService
{
    public Task<PaginatedResponseDto<PizzaDTO>> GetPizzasAsync(int page = 1);

    public Task<PizzaDTO?> GetPizzaAsync(int id);

    public Task<IEnumerable<DoughsDTO>> GetPizzaDoughsAsync();

    public Task<IEnumerable<IngredientDTO>> GetPizzaIngredientsAsync(int id);

    public Task CreatePizzaAsync(CreatePizzaDTO pizzaDTO);

    public Task UpdatePizzaAsync(int id, CreatePizzaDTO pizzaDTO);

    public Task UpdatePizzaAddIngredientAsync(int id, IngredientDTO ingredientDTO);

    public Task UpdatePizzaUpdateIngredientAsync(int id, int ingredientId, IngredientDTO ingredientDTO);

    public Task UpdatePizzaDeleteIngredientAsync(int id, int ingredientId);

    public Task DeletePizzaAsync(int id);
}