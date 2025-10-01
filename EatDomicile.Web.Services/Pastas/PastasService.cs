using EatDomicile.Web.Services.Pastas.DTO;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Pastas;

public class PastasService
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
        var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/pastas", pastaDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePastaAsync(int id, PastaDTO pastaDto)
    {
        var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/pastas/{id}", pastaDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeletePastaAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/pastas/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
