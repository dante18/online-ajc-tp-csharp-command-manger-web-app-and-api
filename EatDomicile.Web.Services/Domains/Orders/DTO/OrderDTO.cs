using EatDomicile.Web.Services.Domains.Users.DTO;

namespace EatDomicile.Web.Services.Domains.Orders.DTO;

public class OrderDTO
{
    public int? Id { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int Status { get; set; }

    public UserDTO User { get; set; }
}
