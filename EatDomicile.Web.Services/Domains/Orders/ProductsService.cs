using System.Net.Http.Json;
using EatDomicile.Web.Services.Domains.Orders.DTO;
using EatDomicile.Web.Services.Interfaces;

namespace EatDomicile.Web.Services.Domains.Orders;

public class ProductsService : IApiProductsService
{
    private readonly HttpClient httpClient;

    public ProductsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync()
    {
        var products = await httpClient.GetFromJsonAsync<IEnumerable<ProductDto>>("https://localhost:7001/api/products");
        return products ?? [];
    }

    public async Task<ProductDto?> GetProductAsync(int id)
    {
        var product = await httpClient.GetFromJsonAsync<ProductDto>($"https://localhost:7001/api/products/{id}");
        return product;
    }
}
