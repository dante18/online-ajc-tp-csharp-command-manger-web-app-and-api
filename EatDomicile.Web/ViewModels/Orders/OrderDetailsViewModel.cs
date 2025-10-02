using EatDomicile.Web.Services.Orders.DTO;

namespace EatDomicile.Web.ViewModels.Orders;

public class OrderDetailsViewModel
{
    public OrderDTO Order { get; set; }

    public IEnumerable<OrderProductDto> Products { get; set; }
}
