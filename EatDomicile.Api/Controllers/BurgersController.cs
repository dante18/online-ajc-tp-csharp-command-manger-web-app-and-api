using EatDomicile.Api.Dtos.Burger;
using EatDomicile.Api.Dtos.Ingredient;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BurgersController : Controller
{
    private readonly BurgerService burgerService;

    private readonly IngredientService ingredientService;

    public BurgersController(BurgerService burgerService, IngredientService ingredientService)
    {
        this.burgerService = burgerService;
        this.ingredientService = ingredientService;
    }

    [HttpGet]
    public IResult GetBurgers()
    {
        List<BurgerDto> burgers = this.burgerService.GetAllBurgers().Select(b => new BurgerDto()
        {
            Id = b.Id,
            Vegetarian = b.Vegetarian,
            Name = b.Name,
            Price = b.Price
        }).ToList();

        return Results.Ok(burgers);
    }

    [HttpGet("{id}")]
    public IResult GetBurger([FromRoute] int id)
    {
        Burger burger = this.burgerService.GetBurger(id);
        if (burger is null)
            return Results.NotFound($"Burger not found by id : {id}");

        BurgerDto burgerDto = new BurgerDto()
        {
            Id = burger.Id,
            Vegetarian = burger.Vegetarian,
            Name = burger.Name,
            Price = burger.Price
        };

        return Results.Ok(burgerDto);
    }

    [HttpGet("{id}/ingredients")]
    public IResult GetBurgerIngredients([FromRoute] int id)
    {
        Burger burger = this.burgerService.GetBurger(id);
        if (burger is null)
            return Results.NotFound($"Burger not found by id : {id}");

        List<IngredientDto> ingredients = this.ingredientService.GetAllIngredientsByBurger(id)
            .Select(i => new IngredientDto()
            {
                Id = i.Id,
                Name = i.Name,
                KCal = i.KCal,
                IsAllergen = i.IsAllergen
            }).ToList();

        return Results.Ok(ingredients);
    }

    [HttpPost()]
    public IResult CreateBurger([FromBody] CreateOrUpdateBurgerDto dto)
    {
        var t = 0;
        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        Burger burger = new Burger()
        {
            Name = dto.Name,
            Price = dto.Price,
            Vegetarian = dto.Vegetarian

        };

        this.burgerService.CreateBurger(burger);

        BurgerDto burgerDto = new BurgerDto()
        {
            Id = burger.Id,
            Vegetarian = burger.Vegetarian,
            Name = burger.Name,
            Price = burger.Price
        };

        return Results.Created($"/api/burgers/{burger.Id}", burgerDto);
    }

    [HttpPut("{id}")]
    public IResult UpdateBurger([FromRoute] int id, [FromBody] CreateOrUpdateBurgerDto dto)
    {
        Burger burger = this.burgerService.GetBurger(id);
        if (burger is null)
            return Results.NotFound($"Burger not found by id : {id}");

        if (dto.Name != null) burger.Name = dto.Name;
        if (dto.Price != null) burger.Price = dto.Price;
        if (dto.Vegetarian != null) burger.Vegetarian = dto.Vegetarian;

        this.burgerService.UpdateBurger(burger);

        return Results.NoContent();
    }

    [HttpPost("{id}/ingredients")]
    public IResult UpdateBurgerAddIngredient([FromRoute] int id, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        Burger burger = this.burgerService.GetBurger(id);
        if (burger is null)
            return Results.NotFound($"Burger not found by id : {id}");

        Ingredient ingredient = new Ingredient()
        {
            Name = dto.Name,
            KCal = dto.KCal,
            IsAllergen = dto.IsAllergen,
            BurgerId = id
        };

        this.ingredientService.CreateIngredient(ingredient);

        return Results.NoContent();
    }

    [HttpPut("{id}/ingredients/{ingredientId}")]
    public IResult UpdateBurgerUpdateIngredient([FromRoute] int id, [FromRoute] int ingredientId, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        Burger burger = this.burgerService.GetBurger(id);
        if (burger is null)
            return Results.NotFound($"Burger not found by id : {id}");

        Ingredient ingredient = this.ingredientService.GetIngredient(ingredientId);
        if (ingredient is null)
            return Results.NotFound($"Ingredient not found by id : {ingredientId}");

        if (dto.Name != null) ingredient.Name = dto.Name;
        if (dto.KCal != null) ingredient.KCal = dto.KCal;
        if (dto.IsAllergen != null) ingredient.IsAllergen = dto.IsAllergen;

        this.ingredientService.UpdateIngredient(ingredient);

        return Results.NoContent();
    }

    [HttpDelete("{id}/ingredients/{ingredientId}")]
    public IResult UpdateBurgerDeleteIngredient([FromRoute] int id, [FromRoute] int ingredientId)
    {
        Burger burger = this.burgerService.GetBurger(id);
        if (burger is null)
            return Results.NotFound($"Burger not found by id : {id}");

        Ingredient ingredient = this.ingredientService.GetIngredient(ingredientId);
        if (ingredient is null)
            return Results.NotFound($"Ingredient not found by id : {ingredientId}");

        this.ingredientService.DeleteIngredient(ingredient);

        return Results.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteBurger(int id)
    {
        Burger burger = this.burgerService.GetBurger(id);
        if (burger is null)
            return Results.NotFound($"Burger not found by id : {id}");

        this.burgerService.DeleteBurger(burger);

        return Results.NoContent();
    }
}
