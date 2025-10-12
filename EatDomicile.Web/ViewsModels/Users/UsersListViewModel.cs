namespace EatDomicile.Web.ViewsModels.Users;

public class UserListViewModel
{
    public List<UsersViewModel> Items { set; get; }

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
