using EatDomicile.Core.Context;
using EatDomicile.Core.Seeders;
using EatDomicile.Core.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<CommandStoreContext>(options => options.UseSqlServer());
builder.Services.AddTransient<BurgerService>();
builder.Services.AddTransient<DoughsService>();
builder.Services.AddTransient<DrinkService>();
builder.Services.AddTransient<FoodService>();
builder.Services.AddTransient<IngredientService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<PastaService>();
builder.Services.AddTransient<PizzaService>();
builder.Services.AddTransient<ProductService>();
builder.Services.AddTransient<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CommandStoreContext>(); 
    DatabaseSeeder.SeedDevData(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
