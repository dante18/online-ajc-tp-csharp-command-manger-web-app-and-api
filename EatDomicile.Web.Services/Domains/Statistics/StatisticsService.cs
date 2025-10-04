using System.Net.Http.Json;
using EatDomicile.Web.Services.Domains.Statistics.DTO;
using EatDomicile.Web.Services.Interfaces;

namespace EatDomicile.Web.Services.Domains.Statistics;

public class StatisticsService : IApiStatisticsService
{
    private readonly HttpClient httpClient;

    public StatisticsService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<OrderDistributions> GetTotalOrdersAsync()
    {
        var totalOrders = await httpClient.GetFromJsonAsync<OrderDistributions>("https://localhost:7001/api/statistics/total-orders");
        return totalOrders ?? new OrderDistributions()
        {
            Label = "totalOrders",
            Value = 0
        };
    }

    public async Task<IEnumerable<OrderDistributions>> GetTotalOrdersByStatusAsync()
    {
        var totalOrdersByStatuts = await httpClient.GetFromJsonAsync<IEnumerable<OrderDistributions>>("https://localhost:7001/api/statistics/orders-by-status");
        return totalOrdersByStatuts ?? [];
    }

    public async Task<IEnumerable<OrderDistributions>> GetTotalOrdersByUserAsync()
    {
        var totalOrdersByUsers = await httpClient.GetFromJsonAsync<IEnumerable<OrderDistributions>>("https://localhost:7001/api/statistics/orders-by-user");
        return totalOrdersByUsers ?? [];
    }

    public async Task<IEnumerable<OrderDistributions>> GetTotalOrdersByDayAsync()
    {
        var totalOrdersByDay = await httpClient.GetFromJsonAsync<IEnumerable<OrderDistributions>>("https://localhost:7001/api/statistics/orders-by-day");
        return totalOrdersByDay ?? [];
    }

    public async Task<IEnumerable<OrderDistributions>> GetUserWitheMoreOrderAsync()
    {
        var usersWithMoreOrder = await httpClient.GetFromJsonAsync<IEnumerable<OrderDistributions>>("https://localhost:7001/api/statistics/top-users");
        return usersWithMoreOrder ?? [];
    }

    public async Task<OrderDistributions> GetOrderAverageDeliveryTimeHoursAsync()
    {
        var ordersDeliveryTimeHours = await httpClient.GetFromJsonAsync<OrderDistributions>("https://localhost:7001/api/statistics/average-delivery-time");
        return ordersDeliveryTimeHours ?? new OrderDistributions()
        {
            Label = "AverageDeliveryTimeHours",
            ValueDouble = 0
        };
    }
}
