using EatDomicile.Api.Dtos.Common;
using EatDomicile.Api.Dtos.Pasta;
using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PastasController : ControllerBase
{
    private readonly ILogger<PastasController> logger;

    private readonly IPastaService pastaService;

    public PastasController(ILogger<PastasController> logger, IPastaService pastaService)
    {
        this.logger = logger;
        this.pastaService = pastaService;
        this.logger.LogInformation("PastasController initialized");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> GetPastas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        this.logger.LogInformation("GetPastas called");

        try
        {
            var queryResult = this.pastaService.GetAllPastas();
            var queryResultWithPagination = await queryResult.ToPagedResultAsync(pageNumber, pageSize);

            List<PastaDto> pastas = queryResultWithPagination.Items.Select(p => new PastaDto()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Type = p.Type,
                KCal = p.KCal,
                Vegetarian = p.Vegetarian,
            }).ToList();

            PaginatedResponse<PastaDto> response = new PaginatedResponse<PastaDto>()
            {
                Items = pastas,
                currentPage = pageNumber,
                maxPage = queryResultWithPagination.TotalPages,
                nextPage = pageNumber == queryResultWithPagination.TotalPages
                    ? queryResultWithPagination.TotalPages
                    : (pageNumber + 1),
                previousPage = pageNumber == 1 ? 1 : (pageNumber - 1)
            };

            this.logger.LogInformation("GetPastas completed successfully. Found {PastaCount} pastas", pastas.Count);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching pastas");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IResult GetPasta([FromRoute] int id)
    {
        this.logger.LogInformation("GetPasta called with ID: {PastaId}", id);

        try
        {
            Pasta pasta = this.pastaService.GetPasta(id);
            if (pasta is null)
            {
                this.logger.LogWarning("Pasta not found with ID: {PastaId}", id);
                return Results.NotFound($"Pasta not found by id : {id}");
            }

            this.logger.LogInformation("GetPasta completed successfully for ID: {PastaId}, Name: {PastaName}",
                id,
                pasta.Name);

            PastaDto pastaDto = new PastaDto()
            {
                Id = pasta.Id,
                Name = pasta.Name,
                Price = pasta.Price,
                Type = pasta.Type,
                KCal = pasta.KCal,
                Vegetarian = pasta.Vegetarian,
            };

            return Results.Ok(pastaDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching pasta with ID: {PastaId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost()]
    [Authorize(Roles = "Admin")]
    public IResult CreatePasta([FromBody] CreateOrUpdatePastaDto dto)
    {
        this.logger.LogInformation("CreatePasta called with KCal: {KCal}, Name: {Name}, Price: {Price}, Type: {Type}, Vegetarian: {Vegetarian}",
            dto.KCal,
            dto.Name,
            dto.Price,
            dto.Type,
            dto.Vegetarian);

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Pasta pasta = new Pasta()
            {
                KCal = dto.KCal,
                Name = dto.Name,
                Price = dto.Price,
                Type = dto.Type,
                Vegetarian = dto.Vegetarian
            };

            this.logger.LogDebug("Adding new pasta to context: {PastaDetails}",
                new
                {
                    dto.KCal,
                    dto.Name,
                    dto.Price,
                    dto.Type,
                    dto.Vegetarian
                }
            );

            this.pastaService.CreatePasta(pasta);

            this.logger.LogInformation("CreatePasta completed successfully. New pasta ID: {PastaId}, Name: {Name}",
                pasta.Id,
                pasta.Name);

            PastaDto pastaDto = new PastaDto()
            {
                Id = pasta.Id,
                Name = pasta.Name,
                Price = pasta.Price,
                Type = pasta.Type,
                KCal = pasta.KCal,
                Vegetarian = pasta.Vegetarian,
            };

            return Results.Created($"/api/pastas/{pasta.Id}", pastaDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e,
                "An error occurred while creating pasta with Name: {Name}",
                dto.Name);

            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdatePasta([FromRoute] int id, [FromBody] CreateOrUpdatePastaDto dto)
    {
        this.logger.LogInformation("UpdatePasta called for ID: {PastaId} with data: {UpdateData}",
            id,
            new
            {
                dto.KCal,
                dto.Name,
                dto.Price,
                dto.Type,
                dto.Vegetarian
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Pasta pasta = this.pastaService.GetPasta(id);
            if (pasta is null)
            {
                this.logger.LogWarning("Cannot update pasta - not found with ID: {PastaId}", id);
                return Results.NotFound($"Pasta not found by id : {id}");
            }

            this.logger.LogDebug("Found existing pasta: {PastaName}",
                pasta.Name);

            if (dto.KCal != null) pasta.KCal = dto.KCal;
            if (dto.Name != null) pasta.Name = dto.Name;
            if (dto.Price != null) pasta.Price = dto.Price;
            if (dto.Type != null) pasta.Type = dto.Type;
            if (dto.Vegetarian != null) pasta.Vegetarian = dto.Vegetarian;

            this.pastaService.UpdatePasta(pasta);

            this.logger.LogInformation("UpdatePasta completed successfully for ID: {PastaId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating pasta with ID: {PastaId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IResult> DeletePasta(int id)
    {
        this.logger.LogInformation("DeletePasta called for ID: {PastaId}", id);

        try
        {
            Pasta pasta = this.pastaService.GetPasta(id);
            if (pasta is null)
            {
                this.logger.LogWarning("Cannot delete pasta - not found with ID: {PastaId}", id);
                return Results.NotFound($"Pasta not found by id : {id}");
            }

            this.logger.LogInformation("Deleting pasta: ID={PastaId}, Name='{PastaName}'",
                pasta.Id,
                pasta.Name);

            this.pastaService.DeletePasta(pasta);

            this.logger.LogInformation("DeletePasta completed successfully for ID: {PastaId}", id);
            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting pasta with ID: {PastaId}", id);
            return Results.InternalServerError(e);
        }
    }
}
