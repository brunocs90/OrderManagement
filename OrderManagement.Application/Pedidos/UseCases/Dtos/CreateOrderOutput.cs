using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class CreateOrderOutput
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string ExternalOrderId { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public List<CreateOrderItemOutput> Items { get; set; } = [];

    public CreateOrderOutput(Order order)
    {
        Id = order.Id;
        OrderNumber = order.OrderNumber;
        ExternalOrderId = order.ExternalOrderId;
        Status = order.Status;
        TotalAmount = order.TotalAmount;
        CreatedAt = order.CreatedAt;
        Items = order.Items.Select(item => new CreateOrderItemOutput(item)).ToList();
    }
}

public class CreateOrderItemOutput
{
    public Guid Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    public CreateOrderItemOutput(OrderItem item)
    {
        Id = item.Id;
        ProductCode = item.ProductCode;
        ProductName = item.ProductName;
        Quantity = item.Quantity;
        UnitPrice = item.UnitPrice;
        TotalPrice = item.TotalPrice;
    }
}