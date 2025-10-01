using EatDomicile.Api.Dtos.Dough;

namespace EatDomicile.Api.Dtos.Pizza;

public class PizzaDto
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public DoughsDto Doughs { get; set; }

    public bool Vegetarian { get; set; }
}
