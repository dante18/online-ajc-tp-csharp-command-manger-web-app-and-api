using System.Net.Http.Json;
using EatDomicile.Web.Services.Interfaces;
using EatDomicile.Web.Services.Domains.Pastas.DTO;

namespace EatDomicile.Web.Services.Domains.Pastas;

public class PastasService : IApiPastasService
{
    private readonly HttpClient httpClient;

    public PastasService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<IEnumerable<PastaDTO>> GetPastasAsync()
    {
        var pastas = await httpClient.GetFromJsonAsync<IEnumerable<PastaDTO>>("https://localhost:7001/api/pastas");
        return pastas ?? [];
    }

    public async Task<PastaDTO?> GetPastaAsync(int id)
    {
        var pasta = await httpClient.GetFromJsonAsync<PastaDTO>($"https://localhost:7001/api/pastas/{id}");
        return pasta;
    }

    public async Task CreatePastaAsync(PastaDTO pastaDto)
    {
        var response = await httpClient.PostAsJsonAsync("https://localhost:7001/api/pastas", pastaDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePastaAsync(int id, PastaDTO pastaDto)
    {
        var response = await httpClient.PutAsJsonAsync($"https://localhost:7001/api/pastas/{id}", pastaDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeletePastaAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"https://localhost:7001/api/pastas/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
