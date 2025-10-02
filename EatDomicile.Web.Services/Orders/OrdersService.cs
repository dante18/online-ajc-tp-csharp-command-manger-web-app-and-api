using System.Net.Http.Json;
using EatDomicile.Web.Services.Orders.DTO;

namespace EatDomicile.Web.Services.Orders;

public class OrdersService
{
    private readonly HttpClient httpClient;

    public OrdersService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<IEnumerable<OrderDTO>> GetOrdersAsync()
    {
        var orders = await httpClient.GetFromJsonAsync<IEnumerable<OrderDTO>>("https://localhost:7001/api/orders");
        return orders ?? [];
    }

    public async Task<OrderDTO?> GetOrderAsync(int id)
    {
        var order = await httpClient.GetFromJsonAsync<OrderDTO>($"https://localhost:7001/api/orders/{id}");
        return order;
    }

    public async Task<IEnumerable<OrderProductDto>> GetOrderProductsAsync(int id)
    {
        var products = await httpClient.GetFromJsonAsync<IEnumerable<OrderProductDto>>($"https://localhost:7001/api/orders/{id}/products");
        return products;
    }

    public async Task CreateOrderAsync(OrderCreateOrUpdateDTO orderDTO)
    {
        var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/orders", orderDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOrderAsync(int id, OrderCreateOrUpdateDTO orderDTO)
    {
        var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/orders/{id}", orderDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOrderAddProductAsync(int id, ProductDto productDto)
    {
        var response = await this.httpClient.PostAsJsonAsync($"https://localhost:7001/api/orders/{id}/products", productDto);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOrderDeleteProductAsync(int id, int productId)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/orders/{id}/products/{productId}");
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteOrderAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/orders/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
