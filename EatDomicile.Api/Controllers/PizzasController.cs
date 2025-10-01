using EatDomicile.Api.Dtos.Dough;
using EatDomicile.Api.Dtos.Ingredient;
using EatDomicile.Api.Dtos.Pizza;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PizzasController : Controller
{
    private readonly PizzaService pizzaService;

    private readonly IngredientService ingredientService;

    private readonly DoughsService doughsService;

    public PizzasController(PizzaService pizzaService, IngredientService ingredientService, DoughsService doughsService)
    {
        this.pizzaService = pizzaService;
        this.ingredientService = ingredientService;
        this.doughsService = doughsService;
    }

    [HttpGet]
    public IResult GetPizzas()
    {
        List<PizzaDto> pizzas = this.pizzaService.GetAllPizzas().Select(p => new PizzaDto()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Doughs = new DoughsDto()
            {
                Id = p.Doughs.Id ?? 0,
                Name = p.Doughs.Name,
            },
            Vegetarian = p.Vegetarian,
        }).ToList();

        return Results.Ok(pizzas);
    }

    [HttpGet("{id}")]
    public IResult GetPizza([FromRoute] int id)
    {
        Pizza pizza = this.pizzaService.GetPizza(id);
        if (pizza is null)
            return Results.NotFound($"Pizza not found by id : {id}");


        PizzaDto pizzaDto = new PizzaDto()
        {
            Id = pizza.Id,
            Name = pizza.Name,
            Price = pizza.Price,
            Doughs = new DoughsDto()
            {
                Id = pizza.Doughs.Id ?? 0,
                Name = pizza.Doughs.Name,
            },
            Vegetarian = pizza.Vegetarian,
        };

        return Results.Ok(pizzaDto);
    }

    [HttpGet("{id}/ingredients")]
    public IResult GetPizzaIngredients([FromRoute] int id)
    {
        Pizza pizza = this.pizzaService.GetPizza(id);
        if (pizza is null)
            return Results.NotFound($"Pizza not found by id : {id}");

        List<IngredientDto> ingredients = this.ingredientService.GetAllIngredientsByPizza(id)
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
    public IResult CreatePizza([FromBody] CreateOrUpdatePizzaDto dto)
    {
        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        Pizza pizza = new Pizza()
        {
            Name = dto.Name,
            Price = dto.Price,
            Vegetarian = dto.Vegetarian,
            DoughsId = dto.DoughsId
        };

        this.pizzaService.CreatePizza(pizza);

        Doughs doughs = this.doughsService.GetDoughs(dto.DoughsId);
        PizzaDto pizzaDto = new PizzaDto()
        {
            Id = pizza.Id,
            Vegetarian = pizza.Vegetarian,
            Name = pizza.Name,
            Price = pizza.Price,
            Doughs = new DoughsDto()
            {
                Id = doughs.Id,
                Name = doughs.Name
            }
        };

        return Results.Created($"/api/pizzas/{pizza.Id}", pizzaDto);
    }

    [HttpPut("{id}")]
    public IResult UpdatePizza([FromRoute] int id, [FromBody] CreateOrUpdatePizzaDto dto)
    {
        Pizza pizza = this.pizzaService.GetPizza(id);
        if (pizza is null)
            return Results.NotFound($"Pizza not found by id : {id}");

        if (dto.Name != null) pizza.Name = dto.Name;
        if (dto.Price != null) pizza.Price = dto.Price;
        if (dto.Vegetarian != null) pizza.Vegetarian = dto.Vegetarian;
        if (dto.DoughsId != null) pizza.DoughsId = dto.DoughsId;

        this.pizzaService.UpdatePizza(pizza);

        return Results.NoContent();
    }

    [HttpPost("{id}/ingredients")]
    public IResult UpdatePizzaAddIngredient([FromRoute] int id, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        Pizza pizza = this.pizzaService.GetPizza(id);
        if (pizza is null)
            return Results.NotFound($"Pizza not found by id : {id}");

        Ingredient ingredient = new Ingredient()
        {
            Name = dto.Name,
            KCal = dto.KCal,
            IsAllergen = dto.IsAllergen,
            PizzaId = id
        };

        this.ingredientService.CreateIngredient(ingredient);

        return Results.NoContent();
    }

    [HttpPut("{id}/ingredients/{ingredientId}")]
    public IResult UpdatePizzaUpdateIngredient([FromRoute] int id, [FromRoute] int ingredientId, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        Pizza pizza = this.pizzaService.GetPizza(id);
        if (pizza is null)
            return Results.NotFound($"Pizza not found by id : {id}");

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
    public IResult UpdatePizzaDeleteIngredient([FromRoute] int id, [FromRoute] int ingredientId)
    {
        Pizza pizza = this.pizzaService.GetPizza(id);
        if (pizza is null)
            return Results.NotFound($"Pizza not found by id : {id}");

        Ingredient ingredient = this.ingredientService.GetIngredient(ingredientId);
        if (ingredient is null)
            return Results.NotFound($"Ingredient not found by id : {ingredientId}");

        this.ingredientService.DeleteIngredient(ingredient);

        return Results.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeletePizza(int id)
    {
        Pizza pizza = this.pizzaService.GetPizza(id);
        if (pizza is null)
            return Results.NotFound($"Pizza not found by id : {id}");

        this.pizzaService.DeletePizza(pizza);

        return Results.NoContent();
    }
}
