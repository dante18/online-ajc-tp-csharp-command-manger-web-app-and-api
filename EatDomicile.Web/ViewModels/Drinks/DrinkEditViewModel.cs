using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Drinks;

public class DrinkEditViewModel
{
    [HiddenInput]
    public int? Id { get; set; }
    public decimal KCal { get; set; }
    [DisplayName("Nom")]
    public string Name { get; set; }
    [DisplayName("Prix")]
    public decimal Price { get; set; }
    [DisplayName("Pétillant")]
    public bool Fizzy { get; set; }
}
