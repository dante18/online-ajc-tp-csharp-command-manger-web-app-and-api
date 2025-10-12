using EatDomicile.Web.Services.Dtos.Application.Statistics;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiStatisticsService
{
    public Task<OrderDistributions> GetTotalOrdersAsync();

    public Task<IEnumerable<OrderDistributions>>? GetTotalOrdersByStatusAsync();

    public Task<IEnumerable<OrderDistributions>> GetTotalOrdersByUserAsync();

    public Task<IEnumerable<OrderDistributions>> GetUserWitheMoreOrderAsync();

    public Task<IEnumerable<OrderDistributions>> GetTotalOrdersByDayAsync();

    public Task<OrderDistributions> GetOrderAverageDeliveryTimeHoursAsync();
}
