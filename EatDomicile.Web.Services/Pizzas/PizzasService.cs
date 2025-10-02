using EatDomicile.Web.Services.Pizzas.DTO;
using System.Net.Http.Json;
using EatDomicile.Web.Services.Interfaces;

namespace EatDomicile.Web.Services.Pizzas;

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

    public async Task CreatePizzaAsync(CreatePizzaDTO pizzaDTO)
    {
        var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/pizzas", pizzaDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePizzaAsync(int id, CreatePizzaDTO pizzaDTO)
    {
        var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/pizzas/{id}", pizzaDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeletePizzaAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/pizzas/{id}");
        _ = response.EnsureSuccessStatusCode();
    }

}
