using System.Net.Http.Json;
using EatDomicile.Web.Services.Domains.Burgers.DTO;
using EatDomicile.Web.Services.Domains.Ingredients.DTO;
using EatDomicile.Web.Services.Interfaces;

namespace EatDomicile.Web.Services.Domains.Burgers;

public class BurgersService : IApiBurgersService
{
    private readonly HttpClient httpClient;

    public BurgersService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<IEnumerable<BurgerDTO>> GetBurgersAsync()
    {
        var burgers = await httpClient.GetFromJsonAsync<IEnumerable<BurgerDTO>>("https://localhost:7001/api/burgers");
        return burgers ?? [];
    }

    public async Task<BurgerDTO?> GetBurgerAsync(int id)
    {
        var burger = await httpClient.GetFromJsonAsync<BurgerDTO>($"https://localhost:7001/api/burgers/{id}");
        return burger;
    }

    public async Task<IEnumerable<IngredientDTO>> GetBurgerIngredientsAsync(int id)
    {
        var ingredients = await httpClient.GetFromJsonAsync<IEnumerable<IngredientDTO>>($"https://localhost:7001/api/burgers/{id}/ingredients");
        return ingredients;
    }

    public async Task CreateBurgerAsync(CreateOrUpdateBurgerDTO burgerDTO)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:7001/api/burgers", burgerDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBurgerAsync(int id, BurgerDTO burgerDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"https://localhost:7001/api/burgers/{id}", burgerDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBurgerAddIngredientAsync(int id, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PostAsJsonAsync($"https://localhost:7001/api/burgers/{id}/ingredients", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBurgerUpdateIngredientAsync(int id, int ingredientId, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"https://localhost:7001/api/burgers/{id}/ingredients/{ingredientId}", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBurgerDeleteIngredientAsync(int id, int ingredientId)
    {
        var response = await httpClient.DeleteAsync($"https://localhost:7001/api/burgers/{id}/ingredients/{ingredientId}");
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteBurgerAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"https://localhost:7001/api/burgers/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
