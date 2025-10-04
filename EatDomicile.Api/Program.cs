using EatDomicile.Api.Extensions;
using EatDomicile.Core.Context;
using EatDomicile.Core.Seeders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEatDomicileApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CommandStoreContext>(); 
    DatabaseSeeder.SeedDevData(context);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
