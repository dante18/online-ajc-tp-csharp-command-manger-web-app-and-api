using EatDomicile.Core.Extensions;

namespace EatDomicile.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEatDomicileApi(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddEatDomicileCore(config);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
