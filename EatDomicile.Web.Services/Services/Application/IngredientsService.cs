using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Ingredients;
using EatDomicile.Web.Services.Dtos.Common;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Services.Application;

internal class IngredientsService : IApiIngredientsService
{
    private readonly HttpClient httpClient;

    public IngredientsService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Ingredients");
    }

    public async Task<PaginatedResponseDto<IngredientDTO>> GetIngredientsAsync(int page = 1)
    {
        var ingredients = await httpClient.GetFromJsonAsync<PaginatedResponseDto<IngredientDTO>>($"?pageNumber={page}");
        return ingredients ?? new PaginatedResponseDto<IngredientDTO>()
        {
            Items = new List<IngredientDTO>(),
            currentPage = 1,
            maxPage = 1,
            nextPage = 1,
            previousPage = 1
        };
    }

    public async Task<IngredientDTO?> GetIngredientAsync(int id)
    {
        var drink = await httpClient.GetFromJsonAsync<IngredientDTO>($"{id}");
        return drink;
    }

    public async Task CreateIngredientAsync(IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PostAsJsonAsync("", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateIngredientAsync(int id, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteIngredientAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
