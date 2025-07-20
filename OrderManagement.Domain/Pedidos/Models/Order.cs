namespace OrderManagement.Domain.Pedidos.Models;

public class Order : ISoftDeletable
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string ExternalOrderId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public decimal TotalAmount { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }
    public DateTimeOffset? CalculatedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }

    public List<OrderItem> Items { get; set; } = [];
}