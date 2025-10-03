namespace EatDomicile.Web.Services.Domains.Burgers.DTO;

public class CreateOrUpdateBurgerDTO
{
    public bool Vegetarian { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }
}
