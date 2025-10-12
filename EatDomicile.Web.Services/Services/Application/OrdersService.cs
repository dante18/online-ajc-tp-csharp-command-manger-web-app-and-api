using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Orders;
using EatDomicile.Web.Services.Dtos.Common;
using System.Net.Http.Json;
using EatDomicile.Web.Services.Dtos.Application.Users;

namespace EatDomicile.Web.Services.Services.Application;

internal class OrdersService : IApiOrdersService
{
    private readonly HttpClient httpClient;

    public OrdersService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Orders");
    }

    public async Task<PaginatedResponseDto<OrderDTO>> GetOrdersAsync(int page = 1)
    {
        var orders = await httpClient.GetFromJsonAsync<PaginatedResponseDto<OrderDTO>>($"?pageNumber={page}");
        return orders ?? new PaginatedResponseDto<OrderDTO>()
        {
            Items = new List<OrderDTO>(),
            currentPage = 1,
            maxPage = 1,
            nextPage = 1,
            previousPage = 1
        };
    }

    public async Task<IEnumerable<UserDTO>> GetOrderClientsAsync()
    {
        var clients = await httpClient.GetFromJsonAsync<IEnumerable<UserDTO>>("order-clients");
        return clients;
    }

    public async Task<OrderDTO?> GetOrderAsync(int id)
    {
        var order = await httpClient.GetFromJsonAsync<OrderDTO>($"{id}");
        return order;
    }

    public async Task<IEnumerable<OrderProductDto>> GetOrderProductsAsync(int id)
    {
        var products = await httpClient.GetFromJsonAsync<IEnumerable<OrderProductDto>>($"{id}/products");
        return products;
    }

    public async Task CreateOrderAsync(OrderCreateOrUpdateDTO orderDTO)
    {
        var response = await httpClient.PostAsJsonAsync("", orderDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOrderAsync(int id, OrderCreateOrUpdateDTO orderDTO)
    {
        var response = await httpClient.PutAsJsonAsync($"{id}", orderDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOrderAddProductAsync(int id, ProductDto productDto)
    {
        var response = await httpClient.PostAsJsonAsync($"{id}/products", productDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOrderDeleteProductAsync(int id, int productId)
    {
        var response = await httpClient.DeleteAsync($"{id}/products/{productId}");
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteOrderAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
