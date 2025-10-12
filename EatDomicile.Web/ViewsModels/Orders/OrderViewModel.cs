using EatDomicile.Web.Services.Dtos.Application.Users;

namespace EatDomicile.Web.ViewsModels.Orders;

public class OrderViewModel
{
    public int? Id { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int Status { get; set; }

    public UserDTO User { get; set; }
}

