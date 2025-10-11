using System.Net.Http.Json;
using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Orders;

namespace EatDomicile.Web.Services.Services.Application;

internal class ProductsService : IApiProductsService
{
    private readonly HttpClient httpClient;

    public ProductsService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Products");
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("");
        return products ?? [];
    }

    public async Task<ProductDto?> GetProductAsync(int id)
    {
        var product = await httpClient.GetFromJsonAsync<ProductDto>($"{id}");
        return product;
    }
}
