namespace EatDomicile.Web.ViewsModels.Doughs;

public class DoughListViewModel
{
    public List<DoughViewModel> Items { set; get; }

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
