namespace EatDomicile.Web.Services.Dtos.Common;

public class PaginatedResponseDto<T>
{
    public List<T> Items { get; set; } = new List<T>();

    public int previousPage { get; set; }

    public int currentPage { get; set; }

    public int nextPage { get; set; }

    public int maxPage { get; set; }
}
