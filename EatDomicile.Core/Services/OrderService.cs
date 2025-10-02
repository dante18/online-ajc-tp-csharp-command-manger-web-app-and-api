using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services;

public sealed class OrderService
{
    private readonly CommandStoreContext _context;

    public OrderService(CommandStoreContext context)
    {
        _context = context;
    }

    public List<Order> GetAllOrders()
    {
        return _context.Orders
            .Include(o => o.User).ThenInclude(ou => ou.Address)
            .Include(o => o.DeliveryAddress)
            .Include(o => o.OrderProduct).ThenInclude(op => op.Product).OrderByDescending(o => o.OrderDate)
            .ToList();
    }

    public Order GetOrder(int id)
    {
        return _context.Orders
            .Include(o => o.User).ThenInclude(ou => ou.Address)
            .Include(o => o.DeliveryAddress)
            .Include(o => o.OrderProduct).ThenInclude(op => op.Product).Where(o => o.Id == id).FirstOrDefault();
    }

    public void CreateOrder(Order order)
    {
        this._context.Orders.Add(order);
        this._context.SaveChanges();
    }

    public void UpdateOrder(Order order)
    {
        this._context.Orders.Update(order);
        this._context.SaveChanges();
    }

    public void UpdateOrderAddProduct(Order order, List<int> productIds)
    {
        order.OrderProduct.Clear();

        foreach (int id in productIds)
        {
            order.OrderProduct.Add(new OrderProduct { ProductId = id });
        }

        this._context.SaveChanges();
    }

    public void UpdateOrderRemoveProduct(Order order, int productId)
    {
        var productToRemove = order.OrderProduct.First(op => op.ProductId == productId);
        order.OrderProduct.Remove(productToRemove);

        this._context.SaveChanges();
    }

    public void DeleteOrder(Order order)
    {
        this._context.Orders.Remove(order);
        this._context.SaveChanges();
    }
}
