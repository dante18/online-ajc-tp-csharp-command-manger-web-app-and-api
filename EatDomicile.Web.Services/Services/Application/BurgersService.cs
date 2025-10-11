using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Burgers;
using EatDomicile.Web.Services.Dtos.Application.Ingredients;
using EatDomicile.Web.Services.Dtos.Common;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Services.Application;

internal class BurgersService : IApiBurgersService
{
    private readonly HttpClient httpClient;

    public BurgersService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Burgers");
    }

    public async Task<PaginatedResponseDto<BurgerDTO>> GetBurgersAsync(int page = 1)
    {
        var burgers = await httpClient.GetFromJsonAsync<PaginatedResponseDto<BurgerDTO>>($"?pageNumber={page}");
        return burgers ?? new PaginatedResponseDto<BurgerDTO>()
        {
            Items = new List<BurgerDTO>(),
            currentPage = 1,
            maxPage = 1,
            nextPage = 1,
            previousPage = 1
        };
    }

    public async Task<BurgerDTO?> GetBurgerAsync(int id)
    {
        var burger = await httpClient.GetFromJsonAsync<BurgerDTO>($"{id}");
        return burger;
    }

    public async Task<IEnumerable<IngredientDTO>> GetBurgerIngredientsAsync(int id)
    {
        var ingredients = await httpClient.GetFromJsonAsync<IEnumerable<IngredientDTO>>($"{id}/ingredients");
        return ingredients;
    }

    public async Task CreateBurgerAsync(CreateOrUpdateBurgerDTO burgerDTO)
    {
        var response = await httpClient.PostAsJsonAsync("", burgerDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBurgerAsync(int id, BurgerDTO burgerDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}", burgerDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBurgerAddIngredientAsync(int id, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PostAsJsonAsync($"{id}/ingredients", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBurgerUpdateIngredientAsync(int id, int ingredientId, IngredientDTO ingredientDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}/ingredients/{ingredientId}", ingredientDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateBurgerDeleteIngredientAsync(int id, int ingredientId)
    {
        var response = await httpClient.DeleteAsync($"{id}/ingredients/{ingredientId}");
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteBurgerAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
