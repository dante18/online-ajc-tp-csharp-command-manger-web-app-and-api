using EatDomicile.Web.Services.Doughs.DTO;


namespace EatDomicile.Web.Services.Pizzas.DTO;

public class PizzaDTO
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public DoughsDTO Doughs { get; set; }

    public bool Vegetarian { get; set; }
}
