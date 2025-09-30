using EatDomicile.Api.Dtos.Dough;
using EatDomicile.Api.Dtos.Drink;
using EatDomicile.Api.Dtos.Pasta;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers
{
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
            List<Doughs> doughs = this.doughService.GetAllDoughs();

            return Results.Ok(doughs);
        }

        [HttpGet("{id}")]
        public IResult GetDough([FromRoute] int id)
        {
            Doughs dough = this.doughService.GetDoughs(id);
            if (dough is null)
                return Results.NotFound($"Dough not found by id : {id}");

            return Results.Ok(dough);
        }

        public IResult CreateDough([FromBody] CreateOrUpdateDoughDto dto)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            Doughs dough = new Doughs()
            {
                Name = dto.Name
            };

            this.doughService.CreateDoughs(dough);

            return Results.Created($"/api/doughs/{dough.Id}", dough);
        }

        [HttpPut("{id}")]
        public IResult UpdateDough([FromRoute] int id, [FromBody] CreateOrUpdateDoughDto dto)
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
}
