using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Orders;

public class OrderEditViewModel
{
    [HiddenInput]
    public int? Id { get; set; }

    [DisplayName("Date de commande")]
    public DateTime OrderDate { get; set; }

    [DisplayName("Date de livraison")]
    public DateTime? DeliveryDate { get; set; }

    [ValidateNever]
    public List<SelectListItem>? UserList { get; set; }

    [DisplayName("Propriétaire")]
    public int? UserId { get; set; }

    [DisplayName("Statut")]
    public int Status { get; set; }
}
