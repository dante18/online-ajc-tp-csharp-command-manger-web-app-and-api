using EatDomicile.Web.Services.Burgers;
using EatDomicile.Web.Services.Doughs;
using EatDomicile.Web.Services.Drinks;
using EatDomicile.Web.Services.Ingredients;
using EatDomicile.Web.Services.Orders;
using EatDomicile.Web.Services.Pastas;
using EatDomicile.Web.Services.Pizzas;
using EatDomicile.Web.Services.Products;
using EatDomicile.Web.Services.Users;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.TryAddTransient<DrinksService>();
builder.Services.TryAddTransient<DoughsService>();
builder.Services.TryAddTransient<IngredientsService>();
builder.Services.TryAddTransient<BurgersService>();
builder.Services.TryAddTransient<PizzasService>();
builder.Services.TryAddTransient<PastasService>();
builder.Services.TryAddTransient<UsersService>();
builder.Services.TryAddTransient<OrdersService>();
builder.Services.TryAddTransient<ProductsService>();
var uriApi = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl");

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
