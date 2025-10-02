using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EatDomicile.Web.ViewModels.Orders;

public class OrderCreateViewModel
{
    [DisplayName("Date de commande")]
    public DateTime OrderDate { get; set; }

    [DisplayName("Date de livraison")]
    public DateTime DeliveryDate { get; set; }

    [ValidateNever]
    public List<SelectListItem> UserList { get; set; }

    [DisplayName("Propriétaire")]
    public int UserId { get; set; }

    [DisplayName("Statut")]
    public int Status { get; set; }
}
