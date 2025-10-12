using EatDomicile.Api.Dtos.Burger;
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
public class BurgersController : Controller
{
    private readonly ILogger<BurgersController> logger;

    private readonly IBurgerService burgerService;

    private readonly IIngredientService ingredientService;

    public BurgersController(ILogger<BurgersController> logger, IBurgerService burgerService, IIngredientService ingredientService)
    {
        this.logger = logger;
        this.burgerService = burgerService;
        this.ingredientService = ingredientService;
        this.logger.LogInformation("BurgersController initialized");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> GetBurgers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        this.logger.LogInformation("GetBurgers called");

        try
        {
            var queryResult = this.burgerService.GetAllBurgers();
            var queryResultWithPagination = await queryResult.ToPagedResultAsync(pageNumber, pageSize);

            List<BurgerDto> burgers = queryResultWithPagination.Items.Select(b => new BurgerDto()
            {
                Id = b.Id,
                Vegetarian = b.Vegetarian,
                Name = b.Name,
                Price = b.Price
            }).ToList();

            PaginatedResponse<BurgerDto> response = new PaginatedResponse<BurgerDto>()
            {
                Items = burgers,
                currentPage = pageNumber,
                maxPage = queryResultWithPagination.TotalPages,
                nextPage = pageNumber == queryResultWithPagination.TotalPages
                    ? queryResultWithPagination.TotalPages
                    : (pageNumber + 1),
                previousPage = pageNumber == 1 ? 1 : (pageNumber - 1)
            };

            this.logger.LogInformation("GetBurgers completed successfully. Found {BurgerCount} burgers", burgers.Count);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching burgers");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IResult GetBurger([FromRoute] int id)
    {
        this.logger.LogInformation("GetBurger called with ID: {BurgerId}", id);

        try
        {
            Burger burger = this.burgerService.GetBurger(id);

            if (burger is null)
            {
                this.logger.LogWarning("Burger not found with ID: {BurgerId}", id);
                return Results.NotFound($"Burger not found by id : {id}");
            }

            this.logger.LogInformation("GetBurger completed successfully for ID: {BurgerId}, Name: {BurgerName}",
                id,
                burger.Name);

            BurgerDto burgerDto = new BurgerDto()
            {
                Id = burger.Id,
                Vegetarian = burger.Vegetarian,
                Name = burger.Name,
                Price = burger.Price
            };

            return Results.Ok(burgerDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching burger with ID: {BurgerId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpGet("{id}/ingredients")]
    [AllowAnonymous]
    public IResult GetBurgerIngredients([FromRoute] int id)
    {
        this.logger.LogInformation("GetBurgerIngredients called with ID: {BurgerId}", id);

        try
        {
            Burger burger = this.burgerService.GetBurger(id);

            if (burger is null)
            {
                this.logger.LogWarning("Burger not found with ID: {BurgerId}", id);
                return Results.NotFound($"Burger not found by id : {id}");
            }

            this.logger.LogInformation("GetBurgerIngredients completed successfully for ID: {BurgerId}, Name: {BurgerName}",
                id,
                burger.Name);

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
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching ingredients burger with ID: {BurgerId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public IResult CreateBurger([FromBody] CreateOrUpdateBurgerDto dto)
    {
        this.logger.LogInformation("CreateBurger called with Name: {Name}, Price: {Price}, Vegetarian: {Vegetarian}",
            dto.Name,
            dto.Price,
            dto.Vegetarian);

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Burger burger = new Burger()
            {
                Name = dto.Name,
                Price = dto.Price,
                Vegetarian = dto.Vegetarian

            };

            this.logger.LogDebug("Adding new burger to context: {BurgerDetails}",
                new
                {
                    dto.Name,
                    dto.Price,
                    dto.Vegetarian
                }
            );

            this.burgerService.CreateBurger(burger);

            this.logger.LogInformation("CreateBurger completed successfully. New burger ID: {BurgerId}, Name: {Name}",
                burger.Id,
                burger.Name);

            BurgerDto burgerDto = new BurgerDto()
            {
                Id = burger.Id,
                Vegetarian = burger.Vegetarian,
                Name = burger.Name,
                Price = burger.Price
            };

            return Results.Created($"/api/burgers/{burger.Id}", burgerDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e,
                "An error occurred while creating burger with Name: {Name}",
                dto.Name);

            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateBurger([FromRoute] int id, [FromBody] CreateOrUpdateBurgerDto dto)
    {
        this.logger.LogInformation("UpdateBurger called for ID: {BurgerId} with data: {UpdateData}",
            id,
            new
            {
                dto.Name,
                dto.Price,
                dto.Vegetarian
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Burger burger = this.burgerService.GetBurger(id);

            if (burger is null)
            {
                this.logger.LogWarning("Cannot update burger - not found with ID: {BurgerId}", id);
                return Results.NotFound($"Burger not found by id : {id}");
            }

            this.logger.LogDebug("Found existing burger: {BurgerName}",
                burger.Name);

            if (dto.Name != null) burger.Name = dto.Name;
            if (dto.Price != null) burger.Price = dto.Price;
            if (dto.Vegetarian != null) burger.Vegetarian = dto.Vegetarian;

            this.burgerService.UpdateBurger(burger);

            this.logger.LogInformation("UpdateBurger completed successfully for ID: {BurgerId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating burger with ID: {BurgerId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost("{id}/ingredients")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateBurgerAddIngredient([FromRoute] int id, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        this.logger.LogInformation("UpdateBurgerAddIngredient called for ID: {BurgerId} with data: {UpdateData}",
            id,
            new
            {
                dto.Name,
                dto.KCal,
                dto.IsAllergen,
                BurgerId = id
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Burger burger = this.burgerService.GetBurger(id);

            if (burger is null)
            {
                this.logger.LogWarning("Cannot add ingredient - burger not found with ID: {BurgerId}", id);
                return Results.NotFound($"Burger not found by id : {id}");
            }

            this.logger.LogDebug("Found existing burger: {BurgerName}",
                burger.Name);

            Ingredient ingredient = new Ingredient()
            {
                Name = dto.Name,
                KCal = dto.KCal,
                IsAllergen = dto.IsAllergen,
                BurgerId = id
            };

            this.ingredientService.CreateIngredient(ingredient);

            this.logger.LogInformation("UpdateBurgerAddIngredient completed successfully for ID: {BurgerId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while adding ingredient to burger with ID: {BurgerId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}/ingredients/{ingredientId}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateBurgerUpdateIngredient([FromRoute] int id, [FromRoute] int ingredientId, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        this.logger.LogInformation("UpdateBurgerUpdateIngredient called for ID: {BurgerId} with data: {UpdateData}",
            id,
            new
            {
                dto.Name,
                dto.KCal,
                dto.IsAllergen
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Burger burger = this.burgerService.GetBurger(id);

            if (burger is null)
            {
                this.logger.LogWarning("Cannot update ingredient - burger not found with ID: {BurgerId}", id);
                return Results.NotFound($"Burger not found by id : {id}");
            }

            Ingredient ingredient = this.ingredientService.GetIngredient(ingredientId);

            if (ingredient is null)
            {
                this.logger.LogWarning("Cannot update ingredient - not found with ID: {BurgeIngredientIdrId}", id);
                return Results.NotFound($"Ingredient not found by id : {id}");
            }

            this.logger.LogDebug("Found existing burger: {BurgerName}",
                burger.Name);

            this.logger.LogDebug("Found existing ingredient: {IngredientName}",
                ingredient.Name);

            if (dto.Name != null) ingredient.Name = dto.Name;
            if (dto.KCal != null) ingredient.KCal = dto.KCal;
            if (dto.IsAllergen != null) ingredient.IsAllergen = dto.IsAllergen;

            this.ingredientService.UpdateIngredient(ingredient);

            this.logger.LogInformation("UpdateBurgerUpdateIngredient completed successfully for ID: {BurgerId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating ingredient to burger with ID: {BurgerId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}/ingredients/{ingredientId}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateBurgerDeleteIngredient([FromRoute] int id, [FromRoute] int ingredientId)
    {
        this.logger.LogInformation("UpdateBurgerDeleteIngredient called for ID: {BurgerId}", id);

        try
        {
            Burger burger = this.burgerService.GetBurger(id);

            if (burger is null)
            {
                this.logger.LogWarning("Cannot delete ingredient - burger not found with ID: {BurgerId}", id);
                return Results.NotFound($"Burger not found by id : {id}");
            }

            Ingredient ingredient = this.ingredientService.GetIngredient(ingredientId);

            if (ingredient is null)
            {
                this.logger.LogWarning("Cannot delete ingredient - not found with ID: {IngredientId}", id);
                return Results.NotFound($"Ingredient not found by id : {id}");
            }

            this.logger.LogDebug("Found existing burger: {BurgerName}",
                burger.Name);

            this.logger.LogDebug("Found existing ingredient: {IngredientName}",
                ingredient.Name);

            this.ingredientService.DeleteIngredient(ingredient);

            this.logger.LogInformation("UpdateBurgerDeleteIngredient completed successfully for ID: {BurgerId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting ingredient to burger with ID: {BurgerId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> DeleteBurger(int id)
    {
        this.logger.LogInformation("DeleteBurger called for ID: {BurgerId}", id);

        try
        {
            Burger burger = this.burgerService.GetBurger(id);

            if (burger is null)
            {
                this.logger.LogWarning("Cannot delete burger - not found with ID: {BurgerId}", id);
                return Results.NotFound($"Burger not found by id : {id}");
            }

            this.logger.LogInformation("Deleting burger: ID={BurgerId}, Name='{BurgerName}'",
                burger.Id,
                burger.Name);

            this.burgerService.DeleteBurger(burger);

            this.logger.LogInformation("DeleteBurger completed successfully for ID: {BurgerId}", id);
            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting burger with ID: {BurgerId}", id);
            return Results.InternalServerError(e);
        }
    }
}
