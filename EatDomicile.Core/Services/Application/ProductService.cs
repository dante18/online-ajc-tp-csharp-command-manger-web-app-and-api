using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class ProductService : IProductService
{
    private readonly ApplicationStoreContext _context;

    public ProductService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public List<Product> GetAllProducts()
    {
        return _context.Product.Include(p => p.OrderProduct).ToList();
    }

    public Product GetProduct(int id)
    {
        return _context.Product.Include(p => p.OrderProduct).Where(p => p.Id == id).FirstOrDefault();
    }
}
