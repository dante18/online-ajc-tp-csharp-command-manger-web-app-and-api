using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Burgers;

public class BurgerViewModel
{
    public int? Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }

    [DisplayName("Prix")]
    public decimal Price { get; set; }

    [DisplayName("Végétarien")]
    public bool Vegetarian { get; set; }
}

