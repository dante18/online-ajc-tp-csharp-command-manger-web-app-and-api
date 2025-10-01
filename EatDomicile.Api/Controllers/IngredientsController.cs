using EatDomicile.Api.Dtos.Ingredient;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : Controller
{
    private readonly IngredientService ingredientService;

    public IngredientsController(IngredientService ingredientService)
    {
        this.ingredientService = ingredientService;
    }

    [HttpGet]
    public IResult GetIngredients()
    {
        List<IngredientDto> ingredients = this.ingredientService.GetAllIngredients().Select(i => new IngredientDto()
        {
            Id = i.Id,
            Name = i.Name,
            KCal = i.KCal,
            IsAllergen = i.IsAllergen
        }).ToList();

        return Results.Ok(ingredients);
    }

    [HttpGet("{id}")]
    public IResult GetIngredient([FromRoute] int id)
    {
        Ingredient ingredient = this.ingredientService.GetIngredient(id);
        if (ingredient is null)
            return Results.NotFound($"Ingredient not found by id : {id}");

        IngredientDto ingredientDto = new IngredientDto()
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen
        };

        return Results.Ok(ingredientDto);
    }

    [HttpPost()]
    public IResult CreateIngredient([FromBody] CreateOrUpdateIngredientDto dto)
    {
        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        Ingredient ingredient = new Ingredient()
        {
            Name = dto.Name,
            KCal = dto.KCal,
            IsAllergen = dto.IsAllergen
        };

        this.ingredientService.CreateIngredient(ingredient);

        IngredientDto ingredientDto = new IngredientDto()
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            KCal = ingredient.KCal,
            IsAllergen = ingredient.IsAllergen
        };

        return Results.Created($"/api/ingredients/{ingredient.Id}", ingredientDto);
    }

    [HttpPut("{id}")]
    public IResult UpdateIngredient([FromRoute] int id, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        Ingredient ingredient = this.ingredientService.GetIngredient(id);
        if (ingredient is null)
            return Results.NotFound($"Ingredient not found by id : {id}");

        if (dto.Name != null) ingredient.Name = dto.Name;
        if (dto.KCal != null) ingredient.KCal = dto.KCal;
        if (dto.IsAllergen != null) ingredient.IsAllergen = dto.IsAllergen;

        this.ingredientService.UpdateIngredient(ingredient);

        return Results.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteIngredient(int id)
    {
        Ingredient ingredient = this.ingredientService.GetIngredient(id);
        if (ingredient is null)
            return Results.NotFound($"Ingredient not found by id : {id}");

        this.ingredientService.DeleteIngredient(ingredient);

        return Results.NoContent();
    }
}
