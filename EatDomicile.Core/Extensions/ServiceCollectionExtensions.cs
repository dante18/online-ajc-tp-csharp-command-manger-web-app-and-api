using EatDomicile.Core.Context;
using EatDomicile.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EatDomicile.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEatDomicileCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<CommandStoreContext>(options => options.UseSqlServer());

        services.AddTransient<BurgerService>();
        services.AddTransient<DoughsService>();
        services.AddTransient<DrinkService>();
        services.AddTransient<FoodService>();
        services.AddTransient<IngredientService>();
        services.AddTransient<OrderService>();
        services.AddTransient<PastaService>();
        services.AddTransient<PizzaService>();
        services.AddTransient<ProductService>();
        services.AddTransient<UserService>();

        return services;
    }
}