using EatDomicile.Api.Dtos.Dough;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoughsController : Controller
{
    private readonly DoughsService doughService;

    public DoughsController(DoughsService doughService)
    {
        this.doughService = doughService;
    }

    [HttpGet]
    public IResult GetDoughs()
    {
        List<DoughsDto> doughs = this.doughService.GetAllDoughs().Select(b => new DoughsDto()
        {
            Id = b.Id,
            Name = b.Name,
        }).ToList();

        return Results.Ok(doughs);
    }

    [HttpGet("{id}")]
    public IResult GetDough([FromRoute] int id)
    {
        Doughs doughs = this.doughService.GetDoughs(id);
        if (doughs is null)
            return Results.NotFound($"Doughs not found by id : {id}");

        DoughsDto doughsDto = new DoughsDto()
        {
            Id = doughs.Id,
            Name = doughs.Name,
        };

        return Results.Ok(doughsDto);
    }

    public IResult CreateDough([FromBody] CreateOrUpdateDoughsDto dto)
    {
        if (!ModelState.IsValid)
            return Results.BadRequest(ModelState);

        Doughs doughs = new Doughs()
        {
            Name = dto.Name
        };

        this.doughService.CreateDoughs(doughs);

        DoughsDto doughsDto = new DoughsDto()
        {
            Id = doughs.Id,
            Name = doughs.Name,
        };

        return Results.Created($"/api/doughs/{doughs.Id}", doughsDto);
    }

    [HttpPut("{id}")]
    public IResult UpdateDough([FromRoute] int id, [FromBody] CreateOrUpdateDoughsDto dto)
    {
        Doughs dough = this.doughService.GetDoughs(id);
        if (dough is null)
            return Results.NotFound($"Dough not found by id : {id}");

        if (dto.Name != null) dough.Name = dto.Name;

        this.doughService.UpdateDoughs(dough);

        return Results.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteDough(int id)
    {
        Doughs dough = this.doughService.GetDoughs(id);
        if (dough is null)
            return Results.NotFound($"Dough not found by id : {id}");

        this.doughService.DeleteDoughs(dough);

        return Results.NoContent();
    }
}
