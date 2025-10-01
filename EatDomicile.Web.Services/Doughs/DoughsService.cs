using System.Net.Http.Json;
using EatDomicile.Web.Services.Doughs.DTO;

namespace EatDomicile.Web.Services.Doughs;

public class DoughsService
{
    private readonly HttpClient httpClient;

    public DoughsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<IEnumerable<DoughsDTO>> GetDoughsAsync()
    {
        var doughs = await httpClient.GetFromJsonAsync<IEnumerable<DoughsDTO>>("https://localhost:7001/api/doughs");
        return doughs ?? [];
    }

    public async Task<DoughsDTO?> GetDoughAsync(int id)
    {
        var dough = await httpClient.GetFromJsonAsync<DoughsDTO>($"https://localhost:7001/api/doughs/{id}");
        return dough;
    }

    public async Task CreateDoughAsync(DoughsDTO doughsDTO)
    {
        var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/doughs", doughsDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateDoughAsync(int id, DoughsDTO doughsDTO)
    {
        var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/doughs/{id}", doughsDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDoughAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/doughs/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
