using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using EatDomicile.Core.Context;

namespace EatDomicile.Core.Factories;

public class ApplicationStoreContextFactory : IDesignTimeDbContextFactory<ApplicationStoreContext>
{
    public ApplicationStoreContext CreateDbContext(string[] args)
    {
        // On se base sur le projet API pour charger la configuration
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../EatDomicile.Api");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("Data");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationStoreContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ApplicationStoreContext(optionsBuilder.Options);
    }
}
