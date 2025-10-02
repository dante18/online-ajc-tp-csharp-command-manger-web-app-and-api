using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Addresses;

public class AddressViewModel
{
    public int? Id { get; set; }
    [DisplayName("Rue")]
    public string Street { get; set; }
    [DisplayName("Ville")]
    public string City { get; set; }
    [DisplayName("Région")]
    public string State { get; set; }
    [DisplayName("Code postal")]
    public string Zip { get; set; }
    [DisplayName("Pays")]
    public string Country { get; set; }
}
