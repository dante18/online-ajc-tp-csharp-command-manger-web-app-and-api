using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Drinks;

public class DrinkDetailsViewModel
{
    public int? Id { get; init; }

    public decimal KCal { get; init; }
    [DisplayName("Nom")]
    public string Name { get; init; }
    [DisplayName("Prix")]
    public decimal Price { get; init; }
    [DisplayName("Pétillant")]
    public bool Fizzy { get; init; }
}
