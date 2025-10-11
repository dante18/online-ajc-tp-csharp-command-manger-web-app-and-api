using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class OrderService : IOrderService
{
    private readonly ApplicationStoreContext _context;

    public OrderService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public IQueryable<Order> GetAllOrders()
    {
        return _context.Orders
            .Include(o => o.User).ThenInclude(ou => ou.Address)
            .Include(o => o.DeliveryAddress)
            .Include(o => o.OrderProduct).ThenInclude(op => op.Product).OrderByDescending(o => o.OrderDate)
            .AsNoTracking();
    }

    public Order GetOrder(int id)
    {
        return _context.Orders
            .Include(o => o.User).ThenInclude(ou => ou.Address)
            .Include(o => o.DeliveryAddress)
            .Include(o => o.OrderProduct).ThenInclude(op => op.Product).Where(o => o.Id == id).FirstOrDefault();
    }

    public Double GetAverageDeliveryTime()
    {
        return _context.Orders.ToList()
            .Where(o => o.DeliveryDate != null)
            .Select(o => (o.DeliveryDate.Value - o.OrderDate).TotalHours)
            .DefaultIfEmpty(0)
            .Average();
    }

    public void CreateOrder(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
    }

    public void UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        _context.SaveChanges();
    }

    public void UpdateOrderAddProduct(Order order, List<int> productIds)
    {
        order.OrderProduct.Clear();

        foreach (int id in productIds)
        {
            order.OrderProduct.Add(new OrderProduct { ProductId = id });
        }

        _context.SaveChanges();
    }

    public void UpdateOrderRemoveProduct(Order order, int productId)
    {
        var productToRemove = order.OrderProduct.First(op => op.ProductId == productId);
        order.OrderProduct.Remove(productToRemove);

        _context.SaveChanges();
    }

    public void DeleteOrder(Order order)
    {
        _context.Orders.Remove(order);
        _context.SaveChanges();
    }
}
