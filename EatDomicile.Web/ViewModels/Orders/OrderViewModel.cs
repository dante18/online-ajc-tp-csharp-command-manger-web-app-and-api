using EatDomicile.Web.Services.Users.DTO;

namespace EatDomicile.Web.ViewModels.Orders;

public class OrderViewModel
{
    public int? Id { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int Status { get; set; }

    public UserDTO User { get; set; }
}

