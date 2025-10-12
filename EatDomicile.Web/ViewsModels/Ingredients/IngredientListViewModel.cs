namespace EatDomicile.Web.ViewsModels.Ingredients;

public class IngredientListViewModel
{
    public List<IngredientViewModel> Items { set; get; }

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
