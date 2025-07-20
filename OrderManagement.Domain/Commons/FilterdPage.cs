using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain;

public class FilterPaged
{
    [Required, DefaultValue(1)]
    public int PageNumber { get; set; } = 1;

    [Required, DefaultValue(10)]
    public int PageSize { get; set; } = 10;
}