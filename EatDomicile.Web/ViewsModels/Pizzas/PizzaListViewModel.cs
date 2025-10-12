namespace EatDomicile.Web.ViewsModels.Pizzas;

public class PizzaListViewModel
{
    public List<PizzaViewModel> Items { set; get; }

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
