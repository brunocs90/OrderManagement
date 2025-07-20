using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class UpdateOrderOutput
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string ExternalOrderId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTimeOffset? CalculatedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }

    public UpdateOrderOutput(Order order)
    {
        Id = order.Id;
        OrderNumber = order.OrderNumber;
        ExternalOrderId = order.ExternalOrderId;
        Status = order.Status;
        TotalAmount = order.TotalAmount;
        CalculatedAt = order.CalculatedAt;
        SentAt = order.SentAt;
    }
}