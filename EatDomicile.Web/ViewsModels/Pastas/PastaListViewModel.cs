namespace EatDomicile.Web.ViewsModels.Pastas;

public class PastaListViewModel
{
    public List<PastaViewModel> Items { set; get; }

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
