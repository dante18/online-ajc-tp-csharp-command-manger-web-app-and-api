using EatDomicile.Api.Dtos.Common;
using EatDomicile.Api.Dtos.Doughs;
using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoughsController : Controller
{
    private readonly ILogger<DoughsController> logger;

    private readonly IDoughsService doughService;

    public DoughsController(ILogger<DoughsController> logger, IDoughsService doughService)
    {
        this.logger = logger;
        this.doughService = doughService;
        this.logger.LogInformation("DoughsController initialized");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IResult> GetDoughs([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        this.logger.LogInformation("GetDoughs called");

        try
        {
            var queryResult = this.doughService.GetAllDoughs();
            var queryResultWithPagination = await queryResult.ToPagedResultAsync(pageNumber, pageSize);

            List<DoughsDto> doughs = queryResultWithPagination.Items.Select(d => new DoughsDto()
            {
                Id = d.Id,
                Name = d.Name,
            }).ToList();

            PaginatedResponse<DoughsDto> response = new PaginatedResponse<DoughsDto>()
            {
                Items = doughs,
                currentPage = pageNumber,
                maxPage = queryResultWithPagination.TotalPages,
                nextPage = pageNumber == queryResultWithPagination.TotalPages
                    ? queryResultWithPagination.TotalPages
                    : (pageNumber + 1),
                previousPage = pageNumber == 1 ? 1 : (pageNumber - 1)
            };

            this.logger.LogInformation("GetDoughs completed successfully. Found {DoughCount} doughs", doughs.Count);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching doughs");

            return Results.InternalServerError();
        }
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IResult GetDough([FromRoute] int id)
    {
        this.logger.LogInformation("GetDough called with ID: {DoughId}", id);

        try
        {
            Doughs dough = this.doughService.GetDoughs(id);

            if (dough is null)
            {
                this.logger.LogWarning("Dough not found with ID: {DoughId}", id);
                return Results.NotFound($"Dough not found by id : {id}");
            }

            this.logger.LogInformation("GetDough completed successfully for ID: {DoughId}, Name: {DoughName}",
                id,
                dough.Name);

            DoughsDto doughsDto = new DoughsDto()
            {
                Id = dough.Id,
                Name = dough.Name,
            };

            return Results.Ok(doughsDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while fetching dough with ID: {DoughId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IResult CreateDough([FromBody] CreateOrUpdateDoughsDto dto)
    {
        this.logger.LogInformation("CreateDough called with Name: {Name}",
            dto.Name);

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Doughs dough = new Doughs()
            {
                Name = dto.Name
            };

            this.logger.LogDebug("Adding new dough to context: {DoughDetails}",
                new
                {
                    dto.Name
                }
            );

            this.doughService.CreateDoughs(dough);

            this.logger.LogInformation("CreateDough completed successfully. New dough ID: {DoughId}, Name: {Name}",
                dough.Id,
                dough.Name);

            DoughsDto doughsDto = new DoughsDto()
            {
                Id = dough.Id,
                Name = dough.Name,
            };

            return Results.Created($"/api/doughs/{dough.Id}", doughsDto);
        }
        catch (Exception e)
        {
            this.logger.LogError(e,
                "An error occurred while creating dough with Name: {Name}",
                dto.Name);

            return Results.InternalServerError(e);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public IResult UpdateDough([FromRoute] int id, [FromBody] CreateOrUpdateDoughsDto dto)
    {
        this.logger.LogInformation("UpdateDough called for ID: {DoughId} with data: {UpdateData}",
            id,
            new
            {
                dto.Name
            });

        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        try
        {
            Doughs dough = this.doughService.GetDoughs(id);

            if (dough is null)
            {
                this.logger.LogWarning("Cannot update dough - not found with ID: {DoughId}", id);
                return Results.NotFound($"Dough not found by id : {id}");
            }

            this.logger.LogDebug("Found existing dough: {DoughName}",
                dough.Name);

            if (dto.Name != null) dough.Name = dto.Name;

            this.doughService.UpdateDoughs(dough);

            this.logger.LogInformation("UpdateDough completed successfully for ID: {DoughId}", id);

            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while updating dough with ID: {DoughId}", id);
            return Results.InternalServerError(e);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteDough(int id)
    {
        this.logger.LogInformation("DeleteDough called for ID: {DoughId}", id);

        try
        {
            Doughs dough = this.doughService.GetDoughs(id);

            if (dough is null)
            {
                this.logger.LogWarning("Cannot delete dough - not found with ID: {DoughId}", id);
                return Results.NotFound($"Dough not found by id : {id}");
            }

            this.logger.LogInformation("Deleting dough: ID={DoughId}, Name='{DoughName}'",
                dough.Id,
                dough.Name);

            this.doughService.DeleteDoughs(dough);

            this.logger.LogInformation("DeleteDough completed successfully for ID: {DoughId}", id);
            
            return Results.NoContent();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "An error occurred while deleting dough with ID: {DoughId}", id);
            return Results.InternalServerError(e);
        }
    }
}
