using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services;

public sealed class ProductService
{
    private readonly CommandStoreContext _context;

    public ProductService(CommandStoreContext context)
    {
        _context = context;
    }

    public List<Product> GetAllProducts()
    {
        return this._context.Product.Include(p => p.OrderProduct).ToList();
    }

    public Product GetProduct(int id)
    {
        return this._context.Product.Include(p => p.OrderProduct).Where(p => p.Id == id).FirstOrDefault();
    }
}
