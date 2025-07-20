using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class GetOrdersInput
{
    public string? OrderNumber { get; set; }
    public string? ExternalOrderId { get; set; }
    public OrderStatus? Status { get; set; }
    public DateTimeOffset? DataCriacaoInicio { get; set; }
    public DateTimeOffset? DataCriacaoFim { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}