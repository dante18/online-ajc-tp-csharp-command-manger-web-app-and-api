using EatDomicile.Web.Services.Orders.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiOrdersService
{
    public Task<IEnumerable<OrderDTO>> GetOrdersAsync();

    public Task<OrderDTO?> GetOrderAsync(int id);

    public Task<IEnumerable<OrderProductDto>> GetOrderProductsAsync(int id);

    public Task CreateOrderAsync(OrderCreateOrUpdateDTO orderDTO);

    public Task UpdateOrderAsync(int id, OrderCreateOrUpdateDTO orderDTO);

    public Task UpdateOrderAddProductAsync(int id, ProductDto productDto);

    public Task UpdateOrderDeleteProductAsync(int id, int productId);

    public Task DeleteOrderAsync(int id);
}