using EatDomicile.Web.Services.Extensions;

namespace EatDomicile.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEatDomicileWeb(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient();
        services.AddEatDomicileService(config);

        return services;
    }
}