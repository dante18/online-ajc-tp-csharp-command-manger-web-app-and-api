using EatDomicile.Web.Services.Ingredient.DTO;
using System.Net.Http.Json;
using EatDomicile.Web.Services.Interfaces;

namespace EatDomicile.Web.Services.Ingredients;
public class IngredientsService : IApiIngredientsService
{
    private readonly HttpClient httpClient;

    public IngredientsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<IEnumerable<IngredientDTO>> GetIngredientsAsync()
    {
        var ingredients = await httpClient.GetFromJsonAsync<IEnumerable<IngredientDTO>>("https://localhost:7001/api/ingredients");
        return ingredients ?? [];
    }

    public async Task<IngredientDTO?> GetIngredientAsync(int id)
    {
        var drink = await httpClient.GetFromJsonAsync<IngredientDTO>($"https://localhost:7001/api/ingredients/{id}");
        return drink;
    }

    public async Task CreateIngredientAsync(IngredientDTO ingredientDTO)
    {
        var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/ingredients", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateIngredientAsync(int id, IngredientDTO ingredientDTO)
    {
        var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/ingredients/{id}", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteIngredientAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/ingredients/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
