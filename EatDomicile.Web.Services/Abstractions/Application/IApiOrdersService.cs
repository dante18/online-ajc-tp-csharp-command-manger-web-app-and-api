using EatDomicile.Web.Services.Dtos.Application.Orders;
using EatDomicile.Web.Services.Dtos.Application.Users;
using EatDomicile.Web.Services.Dtos.Common;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiOrdersService
{
    public Task<PaginatedResponseDto<OrderDTO>> GetOrdersAsync(int page = 1);

    public Task<IEnumerable<UserDTO>> GetOrderClientsAsync();

    public Task<OrderDTO?> GetOrderAsync(int id);

    public Task<IEnumerable<OrderProductDto>> GetOrderProductsAsync(int id);

    public Task CreateOrderAsync(OrderCreateOrUpdateDTO orderDTO);

    public Task UpdateOrderAsync(int id, OrderCreateOrUpdateDTO orderDTO);

    public Task UpdateOrderAddProductAsync(int id, ProductDto productDto);

    public Task UpdateOrderDeleteProductAsync(int id, int productId);

    public Task DeleteOrderAsync(int id);
}