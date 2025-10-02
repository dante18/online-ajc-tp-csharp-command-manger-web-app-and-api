using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace EatDomicile.Web.ViewModels.Orders;

public class OrderProductDeleteViewModel
{
    [HiddenInput]
    public int? Id { get; set; }

    [DisplayName("Nom")]
    public string Name { get; set; }

    [DisplayName("Prix")]
    public decimal Price { get; set; }

    [HiddenInput]
    public int? OrderId { get; set; }
}
