using EatDomicile.Web.Services.Extensions;

namespace EatDomicile.Web.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddEatDomicileWeb(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddEatDomicileService(config);

        return services;
    }
}