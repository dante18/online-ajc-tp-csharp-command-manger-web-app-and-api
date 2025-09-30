using EatDomicile.Api.Dtos.Drink;
using EatDomicile.Api.Dtos.Pizza;
using EatDomicile.Core.Entities;
using EatDomicile.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EatDomicile.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : Controller
    {
        private readonly PizzaService pizzaService;

        public PizzaController(PizzaService pizzaService)
        {
            this.pizzaService = pizzaService;
        }

        [HttpGet]
        public IResult GetPizzas()
        {
            List<Pizza> pizzas = this.pizzaService.GetAllPizzas();

            return Results.Ok(pizzas);
        }

        [HttpGet("{id}")]
        public IResult GetPizza([FromRoute] int id)
        {
            Pizza pizza = this.pizzaService.GetPizza(id);
            if (pizza is null)
                return Results.NotFound($"Pizza not found by id : {id}");

            return Results.Ok(pizza);
        }

        [HttpPost()]
        public IResult CreatePizza([FromBody] CreateOrUpdatePizzaDto dto)
        {
            if (!ModelState.IsValid)
                return Results.BadRequest(ModelState);

            Pizza pizza = new Pizza()
            {
                Name = dto.Name,
                Price = dto.Price
            };

            this.pizzaService.CreatePizza(pizza);

            return Results.Created($"/api/pizzas/{pizza.Id}", pizza);
        }

        [HttpPut("{id}")]
        public IResult UpdatePizza([FromRoute] int id, [FromBody] CreateOrUpdatePizzaDto dto)
        {
            Pizza pizza = this.pizzaService.GetPizza(id);
            if (pizza is null)
                return Results.NotFound($"Pizza not found by id : {id}");

            if (dto.Name != null) pizza.Name = dto.Name;
            if (dto.Price != null) pizza.Price = dto.Price;

            this.pizzaService.UpdatePizza(pizza);

            return Results.NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeletePizza(int id)
        {
            Pizza pizza = this.pizzaService.GetPizza(id);
            if (pizza is null)
                return Results.NotFound($"Pizza not found by id : {id}");

            this.pizzaService.DeletePizza(pizza);

            return Results.NoContent();
        }
    }
}
