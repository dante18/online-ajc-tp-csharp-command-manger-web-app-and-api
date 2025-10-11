using EatDomicile.Api.Dtos.Common;
using EatDomicile.Api.Dtos.Drink;
using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrinksController : ControllerBase
{
    private readonly ILogger<DrinksController> logger;

    private readonly IDrinkService drinkService;

    public DrinksController(ILogger<DrinksController> logger, IDrinkService drinkService)
    {
        this.logger = logger;
        this.drinkService = drinkService;
        this.logger.LogInformation("DrinksController initialized");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> GetDrinks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        this.logger.LogInformation("GetDrinks called");

        try
        {
            var queryResult = this.drinkService.GetAllDrinks();
            var queryResultWithPagination = await queryResult.ToPagedResultAsync(pageNumber, pageSize);

            List<DrinkDto> drinks = queryResultWithPagination.Items.Select(d => new DrinkDto()
            {
                Id = d.Id,
                Name = d.Name,
                Fizzy = d.Fizzy,
                KCal = d.KCal,
                Price = d.Price
            }).ToList();

            PaginatedResponse<DrinkDto> response = new PaginatedResponse<DrinkDto>()
            {
                Items = drinks,
                currentPage = pageNumber,
                maxPage = queryResultWithPagination.TotalPages,
                nextPage = pageNumber == queryResultWithPagination.TotalPages
                    ? queryResultWithPagination.TotalPages
                    : (pageNumber + 1),
                previousPage = pageNumber == 1 ? 1 : (pageNumber - 1)
            };

            this.logger.LogInformation("GetDrinks completed successfully. Found {DrinkCount} drinks", drinks.Count);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching drinks");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public IResult GetDrink([FromRoute] int id)
    {
        this.logger.LogInformation("GetDrink called with ID: {DrinkId}", id);

        try
        {
            Drink drink = this.drinkService.GetDrink(id);

            if (drink is null)
            {
                this.logger.LogWarning("Drink not found with ID: {DrinkId}", id);
                return Results.NotFound($"Drink not found by id : {id}");
            }

            this.logger.LogInformation("GetDrink completed successfully for ID: {DrinkId}, Name: {DrinkName}",
                id,
                drink.Name);

            DrinkDto drinkDto = new DrinkDto()
            {
                Id = drink.Id,
                Name = drink.Name,
                Fizzy = drink.Fizzy,
                KCal = drink.KCal,
                Price = drink.Price
            };

            return Results.Ok(drinkDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching drink with ID: {DrinkId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public IResult CreateDrink([FromBody] CreateOrUpdateDrinkDto dto)
    {
        this.logger.LogInformation("CreateDrink called with Fizzy: {Fizzy}, KCal: {KCal}, Name: {Name}, Price: {Price}",
            dto.Fizzy,
            dto.KCal,
            dto.Name,
            dto.Price);

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Drink drink = new Drink()
            {
                Fizzy = dto.Fizzy,
                KCal = dto.KCal,
                Name = dto.Name,
                Price = dto.Price
            };

            this.logger.LogDebug("Adding new drink to context: {DrinkDetails}",
                new
                {
                    dto.Fizzy,
                    dto.KCal,
                    dto.Name,
                    dto.Price
                }
            );

            this.drinkService.CreateDrink(drink);

            this.logger.LogInformation("CreateDrink completed successfully. New drink ID: {DrinkId}, Name: {Name}",
                drink.Id,
                drink.Name);

            DrinkDto drinkDto = new DrinkDto()
            {
                Id = drink.Id,
                Name = drink.Name,
                Fizzy = drink.Fizzy,
                KCal = drink.KCal,
                Price = drink.Price
            };

            return Results.Created($"/api/drinks/{drink.Id}", drinkDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e,
                "An error occurred while creating drink with Name: {Name}",
                dto.Name);

            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateDrink([FromRoute] int id, [FromBody] CreateOrUpdateDrinkDto dto)
    {
        this.logger.LogInformation("UpdateDrink called for ID: {DrinkId} with data: {UpdateData}",
            id,
            new
            {
                dto.Fizzy,
                dto.KCal,
                dto.Name,
                dto.Price,
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Drink drink = this.drinkService.GetDrink(id);

            if (drink is null)
            {
                this.logger.LogWarning("Cannot update drink - not found with ID: {DrinkId}", id);
                return Results.NotFound($"Drink not found by id : {id}");
            }

            this.logger.LogDebug("Found existing drink: {DrinkName}",
                drink.Name);

            if (dto.Fizzy != null) drink.Fizzy = dto.Fizzy;
            if (dto.KCal != null) drink.KCal = dto.KCal;
            if (dto.Name != null) drink.Name = dto.Name;
            if (dto.Price != null) drink.Price = dto.Price;

            this.drinkService.UpdateDrink(drink);

            this.logger.LogInformation("UpdateDrink completed successfully for ID: {DrinkId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating drink with ID: {DrinkId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult DeleteDrink(int id)
    {
        this.logger.LogInformation("DeleteDrink called for ID: {DrinkId}", id);

        try
        {
            Drink drink = this.drinkService.GetDrink(id);

            if (drink is null)
            {
                this.logger.LogWarning("Cannot delete drink - not found with ID: {DrinkId}", id);
                return Results.NotFound($"Drink not found by id : {id}");
            }

            this.logger.LogInformation("Deleting drink: ID={DrinkId}, Name='{DrinkName}'",
                drink.Id,
                drink.Name);

            this.drinkService.DeleteDrink(drink);

            this.logger.LogInformation("DeleteDrink completed successfully for ID: {DrinkId}", id);
            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting drink with ID: {DrinkId}", id);
            return Results.InternalServerError(e);
        }
    }
}
