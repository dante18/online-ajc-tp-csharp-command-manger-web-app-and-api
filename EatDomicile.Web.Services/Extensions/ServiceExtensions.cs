using EatDomicile.Web.Services.Abstractions.Application;
using EatDomicile.Web.Services.Abstractions.Security;
using EatDomicile.Web.Services.Services.Application;
using EatDomicile.Web.Services.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EatDomicile.Web.Services.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddEatDomicileService(this IServiceCollection services, IConfiguration config)
    {
        var uriApi = config.GetValue<string>("ApiSettings:BaseUrl");

        _ = services.AddTransient<JwtCookieHandler>();

        _ = services.AddHttpClient("Auth", client => client.BaseAddress = new($"{uriApi}/api/auth/"));

        _ = services.AddHttpClient("Drinks", client => client.BaseAddress = new($"{uriApi}/api/drinks/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Doughs", client => client.BaseAddress = new($"{uriApi}/api/doughs/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Ingredients", client => client.BaseAddress = new($"{uriApi}/api/ingredients/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Burgers", client => client.BaseAddress = new($"{uriApi}/api/burgers/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Pizzas", client => client.BaseAddress = new($"{uriApi}/api/pizzas/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Pastas", client => client.BaseAddress = new($"{uriApi}/api/pastas/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Users", client => client.BaseAddress = new($"{uriApi}/api/users/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Orders", client => client.BaseAddress = new($"{uriApi}/api/orders/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Products", client => client.BaseAddress = new($"{uriApi}/api/products/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddHttpClient("Statistics", client => client.BaseAddress = new($"{uriApi}/api/statistics/")).AddHttpMessageHandler<JwtCookieHandler>();

        _ = services.AddScoped<IAuthService, AuthService>();
        _ = services.AddScoped<IApiDrinksService, DrinksService>();
        _ = services.AddScoped<IApiDoughsService, DoughsService>();
        _ = services.AddScoped<IApiIngredientsService, IngredientsService>();
        _ = services.AddScoped<IApiBurgersService, BurgersService>();
        _ = services.AddScoped<IApiPizzasService, PizzasService>();
        _ = services.AddScoped<IApiPastasService, PastasService>();
        _ = services.AddScoped<IApiUsersService, UsersService>();
        _ = services.AddScoped<IApiOrdersService, OrdersService>();
        _ = services.AddScoped<IApiProductsService, ProductsService>();
        _ = services.AddScoped<IApiStatisticsService, StatisticsService>();

        return services;
    }
}
