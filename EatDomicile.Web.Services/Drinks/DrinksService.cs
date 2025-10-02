using EatDomicile.Web.Services.Drinks.DTO;
using EatDomicile.Web.Services.Interfaces;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Drinks;

public class DrinksService : IApiDrinksService
{
    private readonly HttpClient httpClient;

    public DrinksService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<DrinkDTO>> GetDrinksAsync()
    {
        var drinks = await httpClient.GetFromJsonAsync<IEnumerable<DrinkDTO>>("https://localhost:7001/api/drinks");
        return drinks ?? [];
    }

    public async Task<DrinkDTO?> GetDrinkAsync(int id)
    {
        var drink = await httpClient.GetFromJsonAsync<DrinkDTO>($"https://localhost:7001/api/drinks/{id}");
        return drink;
    }

    public async Task CreateDrinkAsync(DrinkDTO drinkDTO)
    {
        var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/drinks", drinkDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateDrinkAsync(int id, DrinkDTO drinkDto)
    {
        var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/drinks/{id}", drinkDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDrinkAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/drinks/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
