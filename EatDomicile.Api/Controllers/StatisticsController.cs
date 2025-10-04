using EatDomicile.Api.Dtos.Statistics;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly OrderService orderService;

    public StatisticsController(OrderService orderService)
    {
        this.orderService = orderService;
    }

    // Nombre total de commandes
    [HttpGet("total-orders")]
    public IResult GetTotalOrders()
    {
        var total = orderService.GetAllOrders().Count();
        return Results.Ok(new OrderDistributions()
        {
            Label = "totalOrders",
            Value = total
        });
    }

    // Nombre de commandes par statut
    [HttpGet("orders-by-status")]
    public IResult GetOrdersByStatus()
    {
        var stats = orderService.GetAllOrders()
            .GroupBy(o => o.Status)
            .Select(g => new OrderDistributions()
            {
                Label = $"{g.Key}",
                Value = g.Count()
            })
            .ToList();

        return Results.Ok(stats);
    }

    // Nombre de commandes par utilisateur
    [HttpGet("orders-by-user")]
    public IResult GetOrdersByUser()
    {
        var stats = orderService.GetAllOrders()
            .GroupBy(o => o.User)
            .Select(g => new OrderDistributions()
            {
                Label = g.Key.FirstName + " " + g.Key.LastName,
                Value = g.Count()
            })
            .OrderByDescending(x => x.Value)
            .ToList();

        return Results.Ok(stats);
    }

    // Top 5 utilisateurs avec le plus de commandes
    [HttpGet("top-users")]
    public IResult GetTopUsers()
    {
        var top = orderService.GetAllOrders()
            .GroupBy(o => o.User)
            .Select(g => new OrderDistributions()
            {
                Label = g.Key.FirstName + " " + g.Key.LastName,
                Value = g.Count()
            })
            .OrderByDescending(x => x.Value)
            .Take(5)
            .ToList();

        return Results.Ok(top);
    }

    // Nombre de commandes par jour
    [HttpGet("orders-by-day")]
    public IResult GetOrdersByDay()
    {
        var stats = orderService.GetAllOrders()
            .GroupBy(o => o.OrderDate.Date)
            .Select(g => new OrderDistributions()
            {
                Label = $"{g.Key}",
                Value = g.Count()
            })
            .OrderBy(x => x.Value)
            .ToList();

        return Results.Ok(stats);
    }

    // Temps moyen de livraison (DeliveryDate - OrderDate)
    [HttpGet("average-delivery-time")]
    public IResult GetAverageDeliveryTime()
    {
        var avg = orderService.GetAllOrders()
            .Where(o => o.DeliveryDate != null)
            .Select(o => (o.DeliveryDate.Value - o.OrderDate).TotalHours)
            .DefaultIfEmpty(0)
            .Average();

        return Results.Ok(new OrderDistributions()
        {
            Label = "AverageDeliveryTimeHours",
            ValueDouble = avg
        });
    }
}