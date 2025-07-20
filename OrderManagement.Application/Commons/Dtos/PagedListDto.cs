namespace OrderManagement.Application.Commons.Dtos;
public class PagedListBase
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int PageCount { get; set; }
    public int TotalCount { get; set; }
}

public sealed class PagedListDto<TType> : PagedListBase
{
    public PagedListDto()
    {
    }
    public PagedListDto(IList<TType> items)
    {
        Items = items;
        TotalCount = items.Count;
    }

    public PagedListDto(PagedListBase pagedResult, IList<TType> items)
    {
        Items = items;
        PageSize = pagedResult.PageSize;
        PageCount = pagedResult.PageCount;
        TotalCount = pagedResult.TotalCount;
        PageNumber = pagedResult.PageNumber;
    }

    public IList<TType> Items { get; set; } = [];
}