using System.Net.Http.Json;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.Services.Domains.Ingredients.DTO;
using EatDomicile.Web.Services.Domains.Pizzas.DTO;

namespace EatDomicile.Web.Services.Domains.Pizzas;

public class PizzasService : IApiPizzasService
{
    private readonly HttpClient httpClient;

    public PizzasService(HttpClient httpClient) 
    {
        this.httpClient = httpClient; 
    }

    public async Task<IEnumerable<PizzaDTO>> GetPizzasAsync()
    {
        var drinks = await httpClient.GetFromJsonAsync<IEnumerable<PizzaDTO>>("https://localhost:7001/api/pizzas");
        return drinks ?? [];
    }

    public async Task<PizzaDTO?> GetPizzaAsync(int id)
    {
        var drink = await httpClient.GetFromJsonAsync<PizzaDTO>($"https://localhost:7001/api/pizzas/{id}");
        return drink;
    }

    public async Task<IEnumerable<IngredientDTO>> GetPizzaIngredientsAsync(int id)
    {
        var ingredients = await httpClient.GetFromJsonAsync<IEnumerable<IngredientDTO>>($"https://localhost:7001/api/pizzas/{id}/ingredients");
        return ingredients;
    }

    public async Task CreatePizzaAsync(CreatePizzaDTO pizzaDTO)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:7001/api/pizzas", pizzaDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaAsync(int id, CreatePizzaDTO pizzaDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"https://localhost:7001/api/pizzas/{id}", pizzaDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaAddIngredientAsync(int id, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PostAsJsonAsync($"https://localhost:7001/api/pizzas/{id}/ingredients", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaUpdateIngredientAsync(int id, int ingredientId, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"https://localhost:7001/api/pizzas/{id}/ingredients/{ingredientId}", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaDeleteIngredientAsync(int id, int ingredientId)
    {
        var response = await httpClient.DeleteAsync($"https://localhost:7001/api/pizzas/{id}/ingredients/{ingredientId}");
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeletePizzaAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"https://localhost:7001/api/pizzas/{id}");
        _ = response.EnsureSuccessStatusCode();
    }

}
