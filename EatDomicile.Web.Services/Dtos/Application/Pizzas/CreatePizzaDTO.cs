namespace EatDomicile.Web.Services.Dtos.Application.Pizzas;

public class CreatePizzaDTO
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public int DoughsId { get; set; }

    public bool Vegetarian { get; set; }
}
