using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class ProcessOrdersOutput
{
    public int TotalProcessed { get; set; }
    public int SuccessCount { get; set; }
    public int ErrorCount { get; set; }
    public List<string> Errors { get; set; } = [];
    public List<ProcessedOrderInfo> ProcessedOrders { get; set; } = [];
}

public class ProcessedOrderInfo
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string ExternalOrderId { get; set; } = string.Empty;
    public OrderStatus PreviousStatus { get; set; }
    public OrderStatus NewStatus { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTimeOffset ProcessedAt { get; set; }
}