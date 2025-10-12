using EatDomicile.Web.Models;
using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.ViewsModels.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EatDomicile.web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IApiStatisticsService statisticsService;

    public HomeController(IApiStatisticsService statisticsService)
    {
        this.statisticsService = statisticsService;
    }

    public async Task<IActionResult> Index()
    {
        var totalOrders = await this.statisticsService.GetTotalOrdersAsync();
        var totalOrdersByStatus = await this.statisticsService.GetTotalOrdersByStatusAsync();
        var totalOrdersByUser = await this.statisticsService.GetTotalOrdersByUserAsync();
        var totalOrdersByDay = await this.statisticsService.GetTotalOrdersByDayAsync();
        var userMoreOrder = await this.statisticsService.GetUserWitheMoreOrderAsync();
        var AverageDeliveryTimeHours = await this.statisticsService.GetOrderAverageDeliveryTimeHoursAsync();

        return View(new StatsViewModel()
        {
            totalNumberOrder = totalOrders.Value,
            totalNumberOrderByStatus = totalOrdersByStatus,
            totalNumberOrderByUser = totalOrdersByUser,
            totalNumberOrderByDay = totalOrdersByDay,
            userMoreOrder = userMoreOrder,
            AverageDeliveryTimeHours = AverageDeliveryTimeHours.ValueDouble ?? 0
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
