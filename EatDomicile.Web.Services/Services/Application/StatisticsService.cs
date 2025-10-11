using System.Net.Http.Json;
using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Dtos.Application.Statistics;

namespace EatDomicile.Web.Services.Services.Application;

internal class StatisticsService : IApiStatisticsService
{
    private readonly HttpClient httpClient;

    public StatisticsService(IHttpClientFactory httpClientFactory)
    {
        this.httpClient = httpClientFactory.CreateClient("Statistics");
    }

    public async Task<OrderDistributions> GetTotalOrdersAsync()
    {
        var totalOrders = await httpClient.GetFromJsonAsync<OrderDistributions>("total-orders");

        return totalOrders ?? new OrderDistributions()
        {
            Label = "totalOrders",
            Value = 0
        };
    }

    public async Task<IEnumerable<OrderDistributions>> GetTotalOrdersByStatusAsync()
    {
        var totalOrdersByStatuts = await httpClient.GetFromJsonAsync<IEnumerable<OrderDistributions>>("orders-by-status");
        return totalOrdersByStatuts ?? [];
    }

    public async Task<IEnumerable<OrderDistributions>> GetTotalOrdersByUserAsync()
    {
        var totalOrdersByUsers = await httpClient.GetFromJsonAsync<IEnumerable<OrderDistributions>>("orders-by-user");
        return totalOrdersByUsers ?? [];
    }

    public async Task<IEnumerable<OrderDistributions>> GetTotalOrdersByDayAsync()
    {
        var totalOrdersByDay = await httpClient.GetFromJsonAsync<IEnumerable<OrderDistributions>>("orders-by-day");
        return totalOrdersByDay ?? [];
    }

    public async Task<IEnumerable<OrderDistributions>> GetUserWitheMoreOrderAsync()
    {
        var usersWithMoreOrder = await httpClient.GetFromJsonAsync<IEnumerable<OrderDistributions>>("top-users");
        return usersWithMoreOrder ?? [];
    }

    public async Task<OrderDistributions> GetOrderAverageDeliveryTimeHoursAsync()
    {
        var ordersDeliveryTimeHours = await httpClient.GetFromJsonAsync<OrderDistributions>("average-delivery-time");
        return ordersDeliveryTimeHours ?? new OrderDistributions()
        {
            Label = "AverageDeliveryTimeHours",
            ValueDouble = 0
        };
    }
}
