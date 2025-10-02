using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Orders;

public class OrderProductAddViewModel
{
    [ValidateNever]
    public List<SelectListItem>? ProductList { get; set; }

    [DisplayName("Propriétaire")]
    public int ProductId { get; set; }
}
