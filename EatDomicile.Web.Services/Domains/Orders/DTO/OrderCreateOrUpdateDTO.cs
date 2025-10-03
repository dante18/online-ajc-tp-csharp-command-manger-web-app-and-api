namespace EatDomicile.Web.Services.Domains.Orders.DTO;

public class OrderCreateOrUpdateDTO
{
    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int? Status { get; set; }

    public int? UserId { get; set; }
}
