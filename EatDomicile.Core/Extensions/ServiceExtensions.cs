using EatDomicile.Core.Abstractions.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EatDomicile.Core.Services.Application;

namespace EatDomicile.Core.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddEatDomicileCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddTransient<IBurgerService, BurgerService>();
        services.AddTransient<IDoughsService, DoughsService>();
        services.AddTransient<IDrinkService, DrinkService>();
        services.AddTransient<IIngredientService, IngredientService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IPastaService, PastaService>();
        services.AddTransient<IPizzaService, PizzaService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<IUserService, UserService>();

        return services;
    }
}