using EatDomicile.Api.Dtos.Common;
using EatDomicile.Api.Dtos.Doughs;
using EatDomicile.Api.Dtos.Ingredient;
using EatDomicile.Api.Dtos.Pizza;
using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PizzasController : Controller
{
    private readonly ILogger<PizzasController> logger;

    private readonly IPizzaService pizzaService;

    private readonly IIngredientService ingredientService;

    private readonly IDoughsService doughsService;

    public PizzasController(ILogger<PizzasController> logger, IPizzaService pizzaService, IIngredientService ingredientService, IDoughsService doughsService)
    {
        this.logger = logger;
        this.pizzaService = pizzaService;
        this.ingredientService = ingredientService;
        this.doughsService = doughsService;
        this.logger.LogInformation("PizzasController initialized");
    }

    
    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> GetPizzas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        this.logger.LogInformation("GetPizzas called");

        try
        {
            var queryResult = this.pizzaService.GetAllPizzas();
            var queryResultWithPagination = await queryResult.ToPagedResultAsync(pageNumber, pageSize);

            List<PizzaDto> pizzas = queryResultWithPagination.Items.Select(p => new PizzaDto()
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

            PaginatedResponse<PizzaDto> response = new PaginatedResponse<PizzaDto>()
            {
                Items = pizzas,
                currentPage = pageNumber,
                maxPage = queryResultWithPagination.TotalPages,
                nextPage = pageNumber == queryResultWithPagination.TotalPages
                    ? queryResultWithPagination.TotalPages
                    : (pageNumber + 1),
                previousPage = pageNumber == 1 ? 1 : (pageNumber - 1)
            };

            this.logger.LogInformation("GetPizzas completed successfully. Found {PizzaCount} pizzas", pizzas.Count);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching pizzas");

            return Results.InternalServerError();
        }
    }

    [HttpGet("pizza-doughs")]
    [Authorize()]
    public async Task<IResult> GetPizzaDoughs()
    {
        this.logger.LogInformation("GetPizzaDoughs called");

        try
        {
            List<DoughsDto> doughs = this.doughsService.GetAllDoughs().Select(b => new DoughsDto()
            {
                Id = b.Id,
                Name = b.Name,
            }).ToList();

            this.logger.LogInformation("GetPizzaDoughs completed successfully. Found {PizzaDoughCount} pizza dough", doughs.Count);

            return Results.Ok(doughs);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching pizza doughs");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IResult GetPizza([FromRoute] int id)
    {
        this.logger.LogInformation("GetPizza called with ID: {PizzaId}", id);

        try
        {
            Pizza pizza = this.pizzaService.GetPizza(id);

            if (pizza is null)
            {
                this.logger.LogWarning("Pizza not found with ID: {PizzaId}", id);
                return Results.NotFound($"Pizza not found by id : {id}");
            }

            this.logger.LogInformation("GetPizza completed successfully for ID: {PizzaId}, Name: {PizzaName}",
                id,
                pizza.Name);

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
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching pizza with ID: {PizzaId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpGet("{id}/ingredients")]
    [AllowAnonymous]
    public IResult GetPizzaIngredients([FromRoute] int id)
    {
        this.logger.LogInformation("GetPizzaIngredients called with ID: {PizzaId}", id);

        try
        {
            Pizza pizza = this.pizzaService.GetPizza(id);

            if (pizza is null)
            {
                this.logger.LogWarning("Pizza not found with ID: {PizzaId}", id);
                return Results.NotFound($"Pizza not found by id : {id}");
            }

            this.logger.LogInformation("GetPizzaIngredients completed successfully for ID: {PizzaId}, Name: {PizzaName}",
                id,
                pizza.Name);

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
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching ingredients pizza with ID: {PizzaId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public IResult CreatePizza([FromBody] CreateOrUpdatePizzaDto dto)
    {
        this.logger.LogInformation("CreatePizza called with Name: {Name}, Price: {Price}, Vegetarian: {Vegetarian}",
            dto.Name,
            dto.Price,
            dto.Vegetarian);

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Pizza pizza = new Pizza()
            {
                Name = dto.Name,
                Price = dto.Price,
                Vegetarian = dto.Vegetarian,
                DoughsId = dto.DoughsId
            };

            this.logger.LogDebug("Adding new pizza to context: {PizzaDetails}",
                new
                {
                    dto.Name,
                    dto.Price,
                    dto.Vegetarian,
                    dto.DoughsId
                }
            );

            this.pizzaService.CreatePizza(pizza);

            this.logger.LogInformation("CreatePizza completed successfully. New pizza ID: {PizzaId}, Name: {Name}",
                pizza.Id,
                pizza.Name);

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
        catch (Exception e)
        {
            this.logger.LogError(e,
                "An error occurred while creating pizza with Name: {Name}",
                dto.Name);

            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdatePizza([FromRoute] int id, [FromBody] CreateOrUpdatePizzaDto dto)
    {
        this.logger.LogInformation("UpdatePizza called for ID: {PizzaId} with data: {UpdateData}",
            id,
            new
            {
                dto.Name,
                dto.Price,
                dto.Vegetarian,
                dto.DoughsId
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Pizza pizza = this.pizzaService.GetPizza(id);

            if (pizza is null)
            {
                this.logger.LogWarning("Cannot update pizza - not found with ID: {PizzaId}", id);
                return Results.NotFound($"Pizza not found by id : {id}");
            }

            this.logger.LogDebug("Found existing pizza: {PizzaName}",
                pizza.Name);

            if (dto.Name != null) pizza.Name = dto.Name;
            if (dto.Price != null) pizza.Price = dto.Price;
            if (dto.Vegetarian != null) pizza.Vegetarian = dto.Vegetarian;
            if (dto.DoughsId != null) pizza.DoughsId = dto.DoughsId;

            this.pizzaService.UpdatePizza(pizza);

            this.logger.LogInformation("UpdatePizza completed successfully for ID: {PizzaId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating pizza with ID: {PizzaId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost("{id}/ingredients")]
    [Authorize(Roles = "Admin")]
    public IResult UpdatePizzaAddIngredient([FromRoute] int id, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        this.logger.LogInformation("UpdatePizzaAddIngredient called for ID: {PizzaId} with data: {UpdateData}",
            id,
            new
            {
                dto.Name,
                dto.KCal,
                dto.IsAllergen,
                PizzaId = id
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Pizza pizza = this.pizzaService.GetPizza(id);

            if (pizza is null)
            {
                this.logger.LogWarning("Cannot add ingredient - pizza not found with ID: {PizzaId}", id);
                return Results.NotFound($"Pizza not found by id : {id}");
            }

            this.logger.LogDebug("Found existing pizza: {PizzaName}",
                pizza.Name);

            Ingredient ingredient = new Ingredient()
            {
                Name = dto.Name,
                KCal = dto.KCal,
                IsAllergen = dto.IsAllergen,
                PizzaId = id
            };

            this.ingredientService.CreateIngredient(ingredient);

            this.logger.LogInformation("UpdatePizzaAddIngredient completed successfully for ID: {PizzaId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while adding ingredient to pizza with ID: {PizzaId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}/ingredients/{ingredientId}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdatePizzaUpdateIngredient([FromRoute] int id, [FromRoute] int ingredientId, [FromBody] CreateOrUpdateIngredientDto dto)
    {
        this.logger.LogInformation("UpdatePizzaUpdateIngredient called for ID: {PizzaId} with data: {UpdateData}",
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
            Pizza pizza = this.pizzaService.GetPizza(id);

            if (pizza is null)
            {
                this.logger.LogWarning("Cannot update ingredient - pizza not found with ID: {PizzaId}", id);
                return Results.NotFound($"Pizza not found by id : {id}");
            }

            Ingredient ingredient = this.ingredientService.GetIngredient(ingredientId);

            if (ingredient is null)
            {
                this.logger.LogWarning("Cannot update ingredient - not found with ID: {PizzaIngredientIdrId}", id);
                return Results.NotFound($"Ingredient not found by id : {id}");
            }

            this.logger.LogDebug("Found existing pizza: {PizzaName}",
                pizza.Name);

            this.logger.LogDebug("Found existing ingredient: {IngredientName}",
                ingredient.Name);

            if (dto.Name != null) ingredient.Name = dto.Name;
            if (dto.KCal != null) ingredient.KCal = dto.KCal;
            if (dto.IsAllergen != null) ingredient.IsAllergen = dto.IsAllergen;

            this.ingredientService.UpdateIngredient(ingredient);

            this.logger.LogInformation("UpdatePizzaUpdateIngredient completed successfully for ID: {PizzaId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating ingredient to pizza with ID: {PizzaId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}/ingredients/{ingredientId}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdatePizzaDeleteIngredient([FromRoute] int id, [FromRoute] int ingredientId)
    {
        this.logger.LogInformation("UpdatePizzaDeleteIngredient called for ID: {PizzaId}", id);

        try
        {
            Pizza pizza = this.pizzaService.GetPizza(id);

            if (pizza is null)
            {
                this.logger.LogWarning("Cannot delete ingredient - pizza not found with ID: {PizzaId}", id);
                return Results.NotFound($"Pizza not found by id : {id}");
            }

            Ingredient ingredient = this.ingredientService.GetIngredient(ingredientId);

            if (ingredient is null)
            {
                this.logger.LogWarning("Cannot delete ingredient - not found with ID: {IngredientId}", id);
                return Results.NotFound($"Ingredient not found by id : {id}");
            }

            this.logger.LogDebug("Found existing pizza: {PizzaName}",
                pizza.Name);

            this.logger.LogDebug("Found existing ingredient: {IngredientName}",
                ingredient.Name);

            this.ingredientService.DeleteIngredient(ingredient);

            this.logger.LogInformation("UpdatePizzaDeleteIngredient completed successfully for ID: {PizzaId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting ingredient to pizza with ID: {PizzaId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> DeletePizza(int id)
    {
        this.logger.LogInformation("DeletePizza called for ID: {PizzaId}", id);

        try
        {
            Pizza pizza = this.pizzaService.GetPizza(id);

            if (pizza is null)
            {
                this.logger.LogWarning("Cannot delete pizza - not found with ID: {PizzaId}", id);
                return Results.NotFound($"Pizza not found by id : {id}");
            }

            this.logger.LogInformation("Deleting pizza: ID={PizzaId}, Name='{PizzaName}'",
                pizza.Id,
                pizza.Name);

            this.pizzaService.DeletePizza(pizza);

            this.logger.LogInformation("DeletePizza completed successfully for ID: {PizzaId}", id);
            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting pizza with ID: {PizzaId}", id);
            return Results.InternalServerError(e);
        }
    }
}
