using EatDomicile.Web.Services.Orders.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiProductsService
{
    public Task<IEnumerable<ProductDto>> GetProductsAsync();

    public Task<ProductDto?> GetProductAsync(int id);
}