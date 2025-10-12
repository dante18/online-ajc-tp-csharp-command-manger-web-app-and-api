using Microsoft.AspNetCore.Identity;
using EatDomicile.Api.Extensions;
using EatDomicile.Core.Context;
using EatDomicile.Core.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container
builder.Services.AddApplicationCoreService(builder.Configuration);
builder.Services.AddApplicationApiService();
builder.Services.AddApplicationApiSecurityServices(builder.Configuration);
builder.Services.AddApplicationApiInfrastructureServices();

// Authentication (JWT)
builder.AddJwtAuthentication();

var app = builder.Build();

// Initialize database
using var scope = app.Services.CreateScope();

try
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationStoreContext>();
    await context.Database.EnsureCreatedAsync();

    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Database initialized successfully.");
}
catch (Exception ex)
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while creating the database.");
    throw;
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    var contextIdentity = scope.ServiceProvider.GetRequiredService<IdentityContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await DatabaseIdentitySeeder.SeedDevDataAsync(contextIdentity, userManager, roleManager);

    var contextApplication = scope.ServiceProvider.GetRequiredService<ApplicationStoreContext>();
    await DatabaseApplicationSeeder.SeedDevDataAsync(contextApplication, userManager, roleManager);

    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Security headers
app.UseHttpsRedirection();

// Response compression (early in pipeline)
app.UseResponseCompression();

// CORS (before authentication)
app.UseCors();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Response caching
app.UseResponseCaching();

// API endpoints
app.MapControllers();

app.Run();