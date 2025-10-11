using EatDomicile.Web.Services.Dtos.Application.Doughs;
using System.ComponentModel;

namespace EatDomicile.Web.ViewsModels.Pizzas;

public class PizzaViewModel
{
    public int? Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }

    [DisplayName("Prix")]
    public decimal Price { get; set; }
    [DisplayName("Pâte")]

    public DoughsDTO Doughs { get; set; }

    [DisplayName("Végétarien")]
    public bool Vegetarian {  get; set; }
}

