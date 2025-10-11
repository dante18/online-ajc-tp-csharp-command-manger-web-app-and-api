using EatDomicile.Web.Services.Dtos.Application.Orders;

namespace EatDomicile.Web.ViewsModels.Orders;

public class OrderDetailsViewModel
{
    public OrderDTO Order { get; set; }

    public IEnumerable<OrderProductDto> Products { get; set; }
}
