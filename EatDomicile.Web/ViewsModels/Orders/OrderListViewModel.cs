namespace EatDomicile.Web.ViewsModels.Orders;

public class OrderListViewModel
{
    public List<OrderViewModel> Items { set; get; }

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
