using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Drinks;
using EatDomicile.Web.Services.Dtos.Common;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Services.Application;

internal class DrinksService : IApiDrinksService
{
    private readonly HttpClient httpClient;

    public DrinksService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Drinks");
    }

    public async Task<PaginatedResponseDto<DrinkDTO>> GetDrinksAsync(int page = 1)
    {
        var drinks = await httpClient.GetFromJsonAsync<PaginatedResponseDto<DrinkDTO>>($"?pageNumber={page}");
        return drinks ?? new PaginatedResponseDto<DrinkDTO>()
        {
            Items = new List<DrinkDTO>(),
            currentPage = 1,
            maxPage = 1,
            nextPage = 1,
            previousPage = 1
        };
    }

    public async Task<DrinkDTO?> GetDrinkAsync(int id)
    {
        var drink = await httpClient.GetFromJsonAsync<DrinkDTO>($"{id}");
        return drink;
    }

    public async Task CreateDrinkAsync(DrinkDTO drinkDTO)
    {
        var response = await httpClient.PostAsJsonAsync("", drinkDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateDrinkAsync(int id, DrinkDTO drinkDto)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}", drinkDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDrinkAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
