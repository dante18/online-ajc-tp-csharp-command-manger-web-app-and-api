using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Burgers;

public class BurgerIngredientViewModel
{
    [HiddenInput]
    public int? Id { get; set; }

    [HiddenInput]
    public int? BurgerId { get; set; }

    [DisplayName("Nom")]

    public string Name { get; set; }
    [DisplayName("KCal")]

    public decimal KCal { get; set; }
    [DisplayName("Allergène")]

    public bool IsAllergen { get; set; }
}