using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class GetOrdersByStatusInput
{
    public OrderStatus Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 100; // Maior page size para integração
    public DateTimeOffset? DataCriacaoInicio { get; set; }
    public DateTimeOffset? DataCriacaoFim { get; set; }

}