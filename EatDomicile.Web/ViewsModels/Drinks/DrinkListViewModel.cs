namespace EatDomicile.Web.ViewsModels.Drinks;

public class DrinkListViewModel
{
    public List<DrinkViewModel> Items { set; get; }

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
