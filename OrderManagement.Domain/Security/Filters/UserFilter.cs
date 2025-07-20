namespace OrderManagement.Domain.Security.Filters;

public class UserFilter : FilterPaged
{
    public Guid? Id { get; set; }
    public string? Login { get; set; }
    public string? Senha { get; set; }
    public bool? Active { get; set; }

    public bool AsNoTracking { get; set; } = true;
}