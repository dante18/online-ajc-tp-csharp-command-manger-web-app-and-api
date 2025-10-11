namespace EatDomicile.Web.ViewsModels.Burgers;

public class BurgerListViewModel
{
    public List<BurgerViewModel> Items { set; get; }

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
