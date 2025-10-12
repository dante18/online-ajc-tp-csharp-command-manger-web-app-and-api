namespace EatDomicile.Web.Services.Dtos.Application.Burgers;

public class BurgerDTO
{
    public int? Id { get; set; }

    public bool Vegetarian { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}
