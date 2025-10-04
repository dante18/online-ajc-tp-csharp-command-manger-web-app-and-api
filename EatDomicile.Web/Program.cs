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
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.TryAddTransient<IApiDrinksService, DrinksService>();
builder.Services.TryAddTransient<IApiDoughsService, DoughsService>();
builder.Services.TryAddTransient<IApiIngredientsService, IngredientsService>();
builder.Services.TryAddTransient<IApiBurgersService, BurgersService>();
builder.Services.TryAddTransient<IApiPizzasService, PizzasService>();
builder.Services.TryAddTransient<IApiPastasService, PastasService>();
builder.Services.TryAddTransient<IApiUsersService, UsersService>();
builder.Services.TryAddTransient<IApiOrdersService, OrdersService>();
builder.Services.TryAddTransient<IApiProductsService, ProductsService>();
builder.Services.TryAddTransient<IApiStatisticsService, StatisticsService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
