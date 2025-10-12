namespace EatDomicile.Web.Services.Dtos.Application.Drinks;

public class DrinkDTO
{
    public int? Id { get; set; }

    public decimal KCal { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public bool Fizzy { get; set; }
}