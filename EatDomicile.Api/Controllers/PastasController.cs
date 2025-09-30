using EatDomicile.Api.Dtos.Drink;
using EatDomicile.Api.Dtos.Pasta;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PastasController : ControllerBase
    {
        private readonly PastaService pastaService;

        public PastasController(PastaService pastaService)
        {
            this.pastaService = pastaService;
        }

        [HttpGet]
        public IResult GetPastas()
        {
            List<Pasta> pastas = this.pastaService.GetAllPastas();

            return Results.Ok(pastas);
        }

        [HttpGet("{id}")]
        public IResult GetPasta([FromRoute] int id)
        {
            Pasta pasta = this.pastaService.GetPasta(id);
            if (pasta is null)
                return Results.NotFound($"Pasta not found by id : {id}");

            return Results.Ok(pasta);
        }

        [HttpPost()]
        public IResult CreatePasta([FromBody] CreateOrUpdatePastaDto dto)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            Pasta pasta = new Pasta()
            {
                KCal = dto.KCal,
                Name = dto.Name,
                Price = dto.Price,
                Type = dto.Type
            };

            this.pastaService.CreatePasta(pasta);

            return Results.Created($"/api/pastas/{pasta.Id}", pasta);
        }
        
        [HttpPut("{id}")]
        public IResult UpdatePasta([FromRoute] int id, [FromBody] CreateOrUpdatePastaDto dto)
        {
            Pasta pasta = this.pastaService.GetPasta(id);
            if (pasta is null)
                return Results.NotFound($"Pasta not found by id : {id}");

            if (dto.KCal != null) pasta.KCal = dto.KCal;
            if (dto.Name != null) pasta.Name = dto.Name;
            if (dto.Price != null) pasta.Price = dto.Price;
            if (dto.Type != null) pasta.Type = dto.Type;

            this.pastaService.UpdatePasta(pasta);

            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeletePasta(int id)
        {
            Pasta pasta = this.pastaService.GetPasta(id);
            if (pasta is null)
                return Results.NotFound($"Pasta not found by id : {id}");

            this.pastaService.DeletePasta(pasta);

            return Results.NoContent();
        }
    }
}
