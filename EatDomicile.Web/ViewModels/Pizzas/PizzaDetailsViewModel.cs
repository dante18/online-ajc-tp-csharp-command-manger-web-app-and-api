using EatDomicile.Web.Services.Doughs.DTO;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Pizzas;

public class PizzaDetailsViewModel
{
    public int? Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }

    [DisplayName("Prix")]
    public decimal Price { get; set; }
    [DisplayName("Pâte")]

    public DoughsDTO Doughs { get; set; }

    [DisplayName("Végétarien")]
    public bool Vegetarian { get; set; }
}
