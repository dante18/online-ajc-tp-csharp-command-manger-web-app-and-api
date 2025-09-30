using EatDomicile.Api.Dtos.Drink;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrinksController : ControllerBase
{
    private readonly DrinkService drinkService;

    public DrinksController(DrinkService drinkService)
    {
        this.drinkService = drinkService;
    }

    [HttpGet]
    public IResult GetDrinks()
    {
        List<Drink> drinks = this.drinkService.GetAllDrinks();

        return Results.Ok(drinks);
    }

    [HttpGet("{id}")]
    public IResult GetDrink([FromRoute] int id)
    {
        Drink drink = this.drinkService.GetDrink(id);
        if (drink is null)
            return Results.NotFound($"Drink not found by id : {id}");

        return Results.Ok(drink);
    }

    [HttpPost()]
    public IResult CreateDrink([FromBody] CreateOrUpdateDrinkDto dto)
    {
        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        Drink drink = new Drink()
        {
            Fizzy = dto.Fizzy,
            KCal = dto.KCal,
            Name = dto.Name,
            Price = dto.Price
        };

        this.drinkService.CreateDrink(drink);

        return Results.Created($"/api/drinks/{drink.Id}", drink);
    }

    [HttpPut("{id}")]
    public IResult UpdateDrink([FromRoute] int id, [FromBody] CreateOrUpdateDrinkDto dto)
    {
        Drink drink = this.drinkService.GetDrink(id);
        if (drink is null)
            return Results.NotFound($"Drink not found by id : {id}");

        if (dto.Fizzy != null) drink.Fizzy = dto.Fizzy;
        if (dto.KCal != null) drink.KCal = dto.KCal;
        if (dto.Name != null) drink.Name = dto.Name;
        if (dto.Price != null) drink.Price = dto.Price;

        this.drinkService.UpdateDrink(drink);

        return Results.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteDrink(int id)
    {
        Drink drink = this.drinkService.GetDrink(id);
        if (drink is null)
            return Results.NotFound($"Drink not found by id : {id}");

        this.drinkService.DeleteDrink(drink);

        return Results.NoContent();
    }
}
