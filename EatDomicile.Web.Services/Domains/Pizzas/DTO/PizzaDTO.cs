using EatDomicile.Web.Services.Domains.Doughs.DTO;

namespace EatDomicile.Web.Services.Domains.Pizzas.DTO;

public class PizzaDTO
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public DoughsDTO Doughs { get; set; }

    public bool Vegetarian { get; set; }
}
