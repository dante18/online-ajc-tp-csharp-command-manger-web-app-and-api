using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Doughs;
using EatDomicile.Web.Services.Dtos.Common;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Services.Application;

internal class DoughsService : IApiDoughsService
{
    private readonly HttpClient httpClient;

    public DoughsService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Doughs");
    }

    public async Task<PaginatedResponseDto<DoughsDTO>> GetDoughsAsync(int page = 1)
    {
        var doughs = await httpClient.GetFromJsonAsync<PaginatedResponseDto<DoughsDTO>>($"?pageNumber={page}");
        return doughs ?? new PaginatedResponseDto<DoughsDTO>()
        {
            Items = new List<DoughsDTO>(),
            currentPage = 1,
            maxPage = 1,
            nextPage = 1,
            previousPage = 1
        };
    }

    public async Task<DoughsDTO?> GetDoughAsync(int id)
    {
        var dough = await httpClient.GetFromJsonAsync<DoughsDTO>($"{id}");
        return dough;
    }

    public async Task CreateDoughAsync(DoughsDTO doughsDTO)
    {
        var response = await httpClient.PostAsJsonAsync("", doughsDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateDoughAsync(int id, DoughsDTO doughsDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}", doughsDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteDoughAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
