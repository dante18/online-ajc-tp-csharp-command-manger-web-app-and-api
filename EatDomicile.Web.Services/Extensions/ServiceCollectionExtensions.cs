using EatDomicile.Web.Services.Domains.Burgers;
using EatDomicile.Web.Services.Domains.Doughs;
using EatDomicile.Web.Services.Domains.Drinks;
using EatDomicile.Web.Services.Domains.Ingredients;
using EatDomicile.Web.Services.Domains.Orders;
using EatDomicile.Web.Services.Domains.Pastas;
using EatDomicile.Web.Services.Domains.Pizzas;
using EatDomicile.Web.Services.Domains.Statistics;
using EatDomicile.Web.Services.Domains.Users;
using EatDomicile.Web.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EatDomicile.Web.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEatDomicileService(this IServiceCollection services, IConfiguration config)
    {
        services.TryAddTransient<IApiDrinksService, DrinksService>();
        services.TryAddTransient<IApiDoughsService, DoughsService>();
        services.TryAddTransient<IApiIngredientsService, IngredientsService>();
        services.TryAddTransient<IApiBurgersService, BurgersService>();
        services.TryAddTransient<IApiPizzasService, PizzasService>();
        services.TryAddTransient<IApiPastasService, PastasService>();
        services.TryAddTransient<IApiUsersService, UsersService>();
        services.TryAddTransient<IApiOrdersService, OrdersService>();
        services.TryAddTransient<IApiProductsService, ProductsService>();
        services.TryAddTransient<IApiStatisticsService, StatisticsService>();

        return services;
    }
}