using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Domain.Pedidos.Filters;

public class OrderFilter : FilterPaged
{
    public string? OrderNumber { get; set; }
    public string? ExternalOrderId { get; set; }
    public OrderStatus? Status { get; set; }
    public DateTimeOffset? DataCriacaoInicio { get; set; }
    public DateTimeOffset? DataCriacaoFim { get; set; }
}