using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Ingredients;
using EatDomicile.Web.Services.Dtos.Application.Pizzas;
using EatDomicile.Web.Services.Dtos.Common;
using System.Net.Http.Json;
using EatDomicile.Web.Services.Dtos.Application.Doughs;

namespace EatDomicile.Web.Services.Services.Application;

internal class PizzasService : IApiPizzasService
{
    private readonly HttpClient httpClient;

    public PizzasService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Pizzas");
    }

    public async Task<PaginatedResponseDto<PizzaDTO>> GetPizzasAsync(int page = 1)
    {
        var drinks = await httpClient.GetFromJsonAsync<PaginatedResponseDto<PizzaDTO>>($"?pageNumber={page}");
        return drinks ?? new PaginatedResponseDto<PizzaDTO>()
        {
            Items = new List<PizzaDTO>(),
            currentPage = 1,
            maxPage = 1,
            nextPage = 1,
            previousPage = 1
        };
    }

    public async Task<IEnumerable<DoughsDTO>> GetPizzaDoughsAsync()
    {
        var doughs = await httpClient.GetFromJsonAsync<IEnumerable<DoughsDTO>>("pizza-doughs");
        return doughs;
    }

    public async Task<PizzaDTO?> GetPizzaAsync(int id)
    {
        var drink = await httpClient.GetFromJsonAsync<PizzaDTO>($"{id}");
        return drink;
    }

    public async Task<IEnumerable<IngredientDTO>> GetPizzaIngredientsAsync(int id)
    {
        var ingredients = await httpClient.GetFromJsonAsync<IEnumerable<IngredientDTO>>($"{id}/ingredients");
        return ingredients;
    }

    public async Task CreatePizzaAsync(CreatePizzaDTO pizzaDTO)
    {
        var response = await httpClient.PostAsJsonAsync("", pizzaDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaAsync(int id, CreatePizzaDTO pizzaDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}", pizzaDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaAddIngredientAsync(int id, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PostAsJsonAsync($"{id}/ingredients", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaUpdateIngredientAsync(int id, int ingredientId, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}/ingredients/{ingredientId}", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaDeleteIngredientAsync(int id, int ingredientId)
    {
        var response = await httpClient.DeleteAsync($"{id}/ingredients/{ingredientId}");
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeletePizzaAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{id}");
        _ = response.EnsureSuccessStatusCode();
    }

}
