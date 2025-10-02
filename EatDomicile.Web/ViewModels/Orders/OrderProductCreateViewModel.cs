using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Orders;

public class OrderProductCreateViewModel
{
    [DisplayName("Nom")]
    public string Name { get; set; }

    [DisplayName("Prix")]
    public decimal Price { get; set; }
}
