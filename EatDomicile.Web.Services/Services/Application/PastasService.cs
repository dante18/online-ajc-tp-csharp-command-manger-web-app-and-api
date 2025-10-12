using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Pastas;
using EatDomicile.Web.Services.Dtos.Common;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Services.Application;

internal class PastasService : IApiPastasService
{
    private readonly HttpClient httpClient;

    public PastasService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Pastas");
    }

    public async Task<PaginatedResponseDto<PastaDTO>> GetPastasAsync(int page = 1)
    {
        var pastas = await httpClient.GetFromJsonAsync<PaginatedResponseDto<PastaDTO>>($"?pageNumber={page}");
        return pastas ?? new PaginatedResponseDto<PastaDTO>()
        {
            Items = new List<PastaDTO>(),
            currentPage = 1,
            maxPage = 1,
            nextPage = 1,
            previousPage = 1
        };
    }

    public async Task<PastaDTO?> GetPastaAsync(int id)
    {
        var pasta = await httpClient.GetFromJsonAsync<PastaDTO>($"{id}");
        return pasta;
    }

    public async Task CreatePastaAsync(PastaDTO pastaDto)
    {
        var response = await httpClient.PostAsJsonAsync("", pastaDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdatePastaAsync(int id, PastaDTO pastaDto)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}", pastaDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeletePastaAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
