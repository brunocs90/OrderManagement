using OrderManagement.Domain.Pedidos.Models;
using OrderManagement.Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class UpdateOrderInput
{
    [Required, EnumValidValue]
    public OrderStatus Status { get; set; }

    public void UpdateModel(Order order)
    {
        order.Status = Status;

        if (Status == OrderStatus.Calculated && order.CalculatedAt == null)
        {
            order.CalculatedAt = DateTimeOffset.UtcNow;
        }

        if (Status == OrderStatus.Sent && order.SentAt == null)
        {
            order.SentAt = DateTimeOffset.UtcNow;
        }
    }
}