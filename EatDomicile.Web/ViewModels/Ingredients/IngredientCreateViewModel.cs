using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Ingredients;

public class IngredientCreateViewModel
{
    [HiddenInput]
    public int? Id { get; set; }
    [DisplayName("Nom")]

    public string Name { get; set; }
    [DisplayName("KCal")]

    public decimal KCal { get; set; }
    [DisplayName("Allergène")]

    public bool IsAllergen { get; set; }
}