using EatDomicile.Api.Dtos.Common;
using EatDomicile.Api.Dtos.Ingredient;
using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : Controller
{
    private readonly ILogger<IngredientsController> logger;

    private readonly IIngredientService ingredientService;

    public IngredientsController(ILogger<IngredientsController> logger, IIngredientService ingredientService)
    {
        this.logger = logger;
        this.ingredientService = ingredientService;
        this.logger.LogInformation("IngredientsController initialized");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> GetIngredients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    { 
        this.logger.LogInformation("GetIngredients called");

        try
        {
            var queryResult = this.ingredientService.GetAllIngredients();
            var queryResultWithPagination = await queryResult.ToPagedResultAsync(pageNumber, pageSize);

            List<IngredientDto> ingredients = queryResultWithPagination.Items.Select(i => new IngredientDto()
            {
                Id = i.Id,
                Name = i.Name,
                KCal = i.KCal,
                IsAllergen = i.IsAllergen
            }).ToList();

            PaginatedResponse<IngredientDto> response = new PaginatedResponse<IngredientDto>()
            {
                Items = ingredients,
                currentPage = pageNumber,
                maxPage = queryResultWithPagination.TotalPages,
                nextPage = pageNumber == queryResultWithPagination.TotalPages
                    ? queryResultWithPagination.TotalPages
                    : (pageNumber + 1),
                previousPage = pageNumber == 1 ? 1 : (pageNumber - 1)
            };

            this.logger.LogInformation("GetIngredients completed successfully. Found {IngredientCount} ingredients", ingredients.Count);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching ingredients");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    public IResult GetIngredient([FromRoute] int id)
    {
        this.logger.LogInformation("GetIngredient called with ID: {IngredientId}", id);

        try
        {
            Ingredient ingredient = this.ingredientService.GetIngredient(id);
            if (ingredient is null)
            {
                this.logger.LogWarning("Ingredient not found with ID: {IngredientId}", id);
                return Results.NotFound($"Ingredient not found by id : {id}");
            }

            this.logger.LogInformation("GetIngredient completed successfully for ID: {IngredientId}, Name: {IngredientName}",
                id,
                ingredient.Name);

            IngredientDto ingredientDto = new IngredientDto()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                KCal = ingredient.KCal,
                IsAllergen = ingredient.IsAllergen
            };

            return Results.Ok(ingredientDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching ingredient with ID: {IngredientId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public IResult CreateIngredient([FromBody] CreateOrUpdateIngredientDto dto)
    {
        this.logger.LogInformation("CreateIngredient called with KCal: {KCal}, Name: {Name}, IsAllergen: {IsAllergen}",
            dto.KCal,
            dto.Name,
            dto.IsAllergen);

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Ingredient ingredient = new Ingredient()
            {
                Name = dto.Name,
                KCal = dto.KCal,
                IsAllergen = dto.IsAllergen
            };

            this.logger.LogDebug("Adding new ingredient to context: {IngredientDetails}",
                new
                {
                    dto.KCal,
                    dto.Name,
                    dto.IsAllergen
                }
            );

            this.ingredientService.CreateIngredient(ingredient);

            this.logger.LogInformation("CreateIngredient completed successfully. New ingredient ID: {IngredientId}, Name: {Name}",
                ingredient.Id,
                ingredient.Name);

            IngredientDto ingredientDto = new IngredientDto()
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                KCal = ingredient.KCal,
                IsAllergen = ingredient.IsAllergen
            };

            return Results.Created($"/api/ingredients/{ingredient.Id}", ingredientDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e,
                "An error occurred while creating ingredient with Name: {Name}",
                dto.Name);

            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateIngredient([FromRoute] int id, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        this.logger.LogInformation("UpdateIngredient called for ID: {IngredientId} with data: {UpdateData}",
            id,
            new
            {
                dto.KCal,
                dto.Name,
                dto.IsAllergen,
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Ingredient ingredient = this.ingredientService.GetIngredient(id);

            if (ingredient is null)
            {
                this.logger.LogWarning("Cannot update ingredient - not found with ID: {IngredientId}", id);
                return Results.NotFound($"Ingredient not found by id : {id}");
            }

            this.logger.LogDebug("Found existing ingredient: {IngredientName}",
                ingredient.Name);

            if (dto.Name != null) ingredient.Name = dto.Name;
            if (dto.KCal != null) ingredient.KCal = dto.KCal;
            if (dto.IsAllergen != null) ingredient.IsAllergen = dto.IsAllergen;

            this.ingredientService.UpdateIngredient(ingredient);

            this.logger.LogInformation("UpdateIngredient completed successfully for ID: {IngredientId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating ingredient with ID: {IngredientId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> DeleteIngredient(int id)
    {
        this.logger.LogInformation("DeleteIngredient called for ID: {IngredientId}", id);

        try
        {
            Ingredient ingredient = this.ingredientService.GetIngredient(id);
            if (ingredient is null)
            {
                this.logger.LogWarning("Cannot update ingredient - not found with ID: {IngredientId}", id);
                return Results.NotFound($"Ingredient not found by id : {id}");
            }

            this.logger.LogInformation("Deleting ingredient: ID={IngredientId}, Name='{IngredientName}'",
                ingredient.Id,
                ingredient.Name);

            this.ingredientService.DeleteIngredient(ingredient);

            this.logger.LogInformation("DeleteIngredient completed successfully for ID: {IngredientId}", id);
            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting ingredient with ID: {IngredientId}", id);
            return Results.InternalServerError(e);
        }
    }
}
