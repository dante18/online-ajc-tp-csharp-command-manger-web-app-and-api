using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IOrderService
{
    public IQueryable<Order> GetAllOrders();

    public Order GetOrder(int id);

    public Double GetAverageDeliveryTime();

    public void CreateOrder(Order order);

    public void UpdateOrder(Order order);

    public void UpdateOrderAddProduct(Order order, List<int> productIds);

    public void UpdateOrderRemoveProduct(Order order, int productId);

    public void DeleteOrder(Order order);
}
