using OrderManagement.Domain.Pedidos.Models;
using OrderManagement.Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.Pedidos.UseCases.Dtos;

public class ProcessOrdersInput
{
    [Required]
    public List<Guid> OrderIds { get; set; } = [];

    [Required, EnumValidValue]
    public OrderStatus TargetStatus { get; set; } = OrderStatus.Calculated;
}