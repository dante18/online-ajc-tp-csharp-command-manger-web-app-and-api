using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EatDomicile.Web.ViewModels.Pastas;

public class PastaCreateViewModel
{
    public string Name { get; set; }
    [DisplayName("Prix")]
    [Range(8.50, 20)]
    public decimal Price { get; set; }
    [DisplayName("Type")]
    public int Type { get; set; }
    [DisplayName("KCal")]
    public decimal KCal { get; set; }
    [DisplayName("Végétarien")]
    public bool Vegetarian { get; set; }
}
