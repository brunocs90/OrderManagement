using OrderManagement.Domain.Pedidos.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class CreateOrderInput
{
    [Required, MaxLength(50)]
    public string ExternalOrderId { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required]
    public List<CreateOrderItemInput> Items { get; set; } = [];

    public Order ToModel()
    {
        var order = new Order
        {
            ExternalOrderId = ExternalOrderId,
            OrderNumber = OrderNumber,
            Status = OrderStatus.Pending,
            TotalAmount = 0  // Será calculado apenas no processamento
        };

        foreach (var item in Items)
        {
            var orderItem = new OrderItem
            {
                ProductCode = item.ProductCode,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice  // Calcula apenas o item individual
            };
            order.Items.Add(orderItem);
            // Não soma ao TotalAmount aqui - será calculado no processamento
        }

        return order;
    }
}

public class CreateOrderItemInput
{
    [Required, MaxLength(50)]
    public string ProductCode { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [Required, Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required, Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}