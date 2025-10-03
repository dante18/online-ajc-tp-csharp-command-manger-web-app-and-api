namespace EatDomicile.Web.Services.Domains.Pizzas.DTO;

public class CreatePizzaDTO
{
    public int? Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public int DoughsId { get; set; }

    public bool Vegetarian { get; set; }
}
