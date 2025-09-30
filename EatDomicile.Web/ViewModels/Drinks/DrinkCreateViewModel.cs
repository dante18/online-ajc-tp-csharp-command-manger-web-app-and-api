using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Drinks;

public class DrinkCreateViewModel
{
    public decimal KCal { get; set; }
    [DisplayName("Nom")]
    public string Name { get; set; }
    [DisplayName("Prix")]
    public decimal Price { get; set; }
    [DisplayName("Pétillant")]
    public bool Fizzy { get; set; }
}
