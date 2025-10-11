namespace EatDomicile.Web.Services.Dtos.Application.Burgers;

public class CreateOrUpdateBurgerDTO
{
    public bool Vegetarian { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}
