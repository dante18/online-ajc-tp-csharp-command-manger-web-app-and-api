using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IProductService
{
    public List<Product> GetAllProducts();

    public Product GetProduct(int id);
}
