using EatDomicile.Api.Dtos.Statistics;
using EatDomicile.Core.Abstractions.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly ILogger<StatisticsController> logger;

    private readonly IOrderService orderService;

    public StatisticsController(ILogger<StatisticsController> logger, IOrderService orderService)
    {
        this.logger = logger;
        this.orderService = orderService;
        this.logger.LogInformation("StatisticsController initialized");
    }

    // Nombre total de commandes
    [HttpGet("total-orders")]
    [Authorize]
    public IResult GetTotalOrders()
    {
        this.logger.LogInformation("GetTotalOrders called");

        try
        {
            var total = orderService.GetAllOrders().Count();

            this.logger.LogInformation("GetTotalOrders completed successfully. Found {total} records", total);

            return Results.Ok(new OrderDistributions()
            {
                Label = "totalOrders",
                Value = total
            });
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching data");

            return Results.InternalServerError();
        }
    }

    // Nombre de commandes par statut
    [HttpGet("orders-by-status")]
    [Authorize]
    public IResult GetOrdersByStatus()
    {
        this.logger.LogInformation("GetOrdersByStatus called");

        try
        {
            var stats = orderService.GetAllOrders()
                .GroupBy(o => o.Status)
                .Select(g => new OrderDistributions()
                {
                    Label = $"{g.Key}",
                    Value = g.Count()
                })
                .ToList();

            this.logger.LogInformation("GetOrdersByStatus completed successfully.");

            return Results.Ok(stats);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching data");

            return Results.InternalServerError();
        }
    }

    // Nombre de commandes par utilisateur
    [HttpGet("orders-by-user")]
    [Authorize]
    public IResult GetOrdersByUser()
    {
        this.logger.LogInformation("GetOrdersByUser called");

        try
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

            this.logger.LogInformation("GetOrdersByUser completed successfully");

            return Results.Ok(stats);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching data");

            return Results.InternalServerError();
        }
    }

    // Top 5 utilisateurs avec le plus de commandes
    [HttpGet("top-users")]
    [Authorize]
    public IResult GetTopUsers()
    {
        this.logger.LogInformation("GetTopUsers called");

        try
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

            this.logger.LogInformation("GetTopUsers completed successfully");

            return Results.Ok(top);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching data");

            return Results.InternalServerError();
        }
    }

    // Nombre de commandes par jour
    [HttpGet("orders-by-day")]
    [Authorize]
    public IResult GetOrdersByDay()
    {
        this.logger.LogInformation("GetOrdersByDay called");

        try
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

            this.logger.LogInformation("GetOrdersByDay completed successfully");

            return Results.Ok(stats);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching data");

            return Results.InternalServerError();
        }
    }

    // Temps moyen de livraison (DeliveryDate - OrderDate)
    [HttpGet("average-delivery-time")]
    [Authorize]
    public IResult GetAverageDeliveryTime()
    {
        this.logger.LogInformation("GetAverageDeliveryTime called");

        try
        {
            var avg = orderService.GetAverageDeliveryTime();

            this.logger.LogInformation("GetAverageDeliveryTime completed successfully");

            return Results.Ok(new OrderDistributions()
            {
                Label = "AverageDeliveryTimeHours",
                ValueDouble = avg
            });
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching data");

            return Results.InternalServerError();
        }
    }
}