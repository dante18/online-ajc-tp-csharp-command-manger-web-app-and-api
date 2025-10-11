using EatDomicile.Web.Services.Dtos.Application.Doughs;

namespace EatDomicile.Web.Services.Dtos.Application.Pizzas;

public class PizzaDTO
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public DoughsDTO Doughs { get; set; }

    public bool Vegetarian { get; set; }
}
