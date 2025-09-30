using EatDomicile.Api.Dtos.Burger;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BurgersController : Controller
    {
        private readonly BurgerService burgerService;
        public BurgersController(BurgerService burgerService)
        {
            this.burgerService = burgerService;
        }

        [HttpGet]
        public IResult GetBurgers()
        {
            List<Burger> burgers = this.burgerService.GetAllBurgers();

            return Results.Ok(burgers);
        }

        [HttpGet("{id}")]
        public IResult GetBurger([FromRoute] int id)
        {
            Burger burger = this.burgerService.GetBurger(id);
            if (burger is null)
                return Results.NotFound($"Burger not found by id : {id}");

            return Results.Ok(burger);
        }

        [HttpPost()]
        public IResult CreateBurger([FromBody] CreateOrUpdateBurgerDto dto)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            Burger burger = new Burger()
            {
                Name = dto.Name,
                Price = dto.Price
            };

            this.burgerService.CreateBurger(burger);

            return Results.Created($"/api/burgers/{burger.Id}", burger);
        }

        [HttpPut("{id}")]
        public IResult UpdateBurger([FromRoute] int id, [FromBody] CreateOrUpdateBurgerDto dto)
        {
            Burger burger = this.burgerService.GetBurger(id);
            if (burger is null)
                return Results.NotFound($"Burger not found by id : {id}");

            if (dto.Name != null) burger.Name = dto.Name;
            if (dto.Price != null) burger.Price = dto.Price;

            this.burgerService.UpdateBurger(burger);

            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteBurger(int id)
        {
            Burger burger = this.burgerService.GetBurger(id);
            if (burger is null)
                return Results.NotFound($"Burger not found by id : {id}");

            this.burgerService.DeleteBurger(burger);

            return Results.NoContent();
        }
    }
}
