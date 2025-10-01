using EatDomicile.Web.Services.Doughs.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Pizzas;

public class PizzaCreateViewModel
{
    public int? Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }

    [DisplayName("Prix")]
    public decimal Price { get; set; }
    [DisplayName("Pâte")]
    [ValidateNever]
    public List<SelectListItem> DoughsList { get; set; }

    [DisplayName("Pâte")]

    public int DoughId { get; set; }

    [DisplayName("Végétarien")]
    public bool Vegetarian { get; set; }
}
