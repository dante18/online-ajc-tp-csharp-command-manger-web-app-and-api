using EatDomicile.Web.Services.Dtos.Application.Orders;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiProductsService
{
    public Task<IEnumerable<ProductDto>> GetProductsAsync();

    public Task<ProductDto?> GetProductAsync(int id);
}