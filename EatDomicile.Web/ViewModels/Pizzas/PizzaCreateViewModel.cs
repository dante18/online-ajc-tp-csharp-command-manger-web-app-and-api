using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EatDomicile.Web.ViewModels.Pizzas;

public class PizzaCreateViewModel
{
    [DisplayName("Nom")]
    public string Name { get; set; }

    [DisplayName("Prix")]
    [Range(1.50, 20)]
    public decimal Price { get; set; }

    [ValidateNever]
    public List<SelectListItem> DoughsList { get; set; }

    [DisplayName("Pâte")]
    public int DoughId { get; set; }

    [DisplayName("Végétarien")]
    public bool Vegetarian { get; set; }
}
