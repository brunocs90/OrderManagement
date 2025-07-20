using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class GetOrderByIdOutput
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string ExternalOrderId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? CalculatedAt { get; set; }
    public DateTimeOffset? SentAt { get; set; }
    public List<GetOrderItemOutput> Items { get; set; } = [];

    public GetOrderByIdOutput(Order order)
    {
        Id = order.Id;
        OrderNumber = order.OrderNumber;
        ExternalOrderId = order.ExternalOrderId;
        Status = order.Status;
        TotalAmount = order.TotalAmount;
        CreatedAt = order.CreatedAt;
        CalculatedAt = order.CalculatedAt;
        SentAt = order.SentAt;
        Items = order.Items.Select(item => new GetOrderItemOutput(item)).ToList();
    }
}

public class GetOrderItemOutput
{
    public Guid Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public GetOrderItemOutput(OrderItem item)
    {
        Id = item.Id;
        ProductCode = item.ProductCode;
        ProductName = item.ProductName;
        Quantity = item.Quantity;
        UnitPrice = item.UnitPrice;
        TotalPrice = item.TotalPrice;
        CreatedAt = item.CreatedAt;
    }
}