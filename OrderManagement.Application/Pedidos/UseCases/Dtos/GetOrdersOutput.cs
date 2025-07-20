using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class GetOrdersOutput
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string ExternalOrderId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? CalculatedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
    public int ItemsCount { get; set; }

    public GetOrdersOutput(Order order)
    {
        Id = order.Id;
        OrderNumber = order.OrderNumber;
        ExternalOrderId = order.ExternalOrderId;
        Status = order.Status;
        TotalAmount = order.TotalAmount;
        CreatedAt = order.CreatedAt;
        CalculatedAt = order.CalculatedAt;
        SentAt = order.SentAt;
        ItemsCount = order.Items.Count;
    }
}