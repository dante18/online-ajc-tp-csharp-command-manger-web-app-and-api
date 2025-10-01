using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Pastas;

public class PastaViewModel
{
    public int? Id { get; set; }
    [DisplayName("Nom")]
    public string Name { get; set; }
    [DisplayName("Prix")]
    public decimal Price { get; set; }
    [DisplayName("Type")]
    public int Type { get; set; }
    [DisplayName("KCal")]
    public decimal KCal { get; set; }
    [DisplayName("Végétarien")]
    public bool Vegetarian { get; set; }
}
