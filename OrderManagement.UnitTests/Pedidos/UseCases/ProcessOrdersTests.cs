using FluentAssertions;
using Moq;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Implementations;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.UnitTests.Pedidos.UseCases;

public class ProcessOrdersTests
{
    private readonly Mock<IOrderDbAdapter> _mockOrderDbAdapter;
    private readonly ProcessOrders _processOrders;

    public ProcessOrdersTests()
    {
        _mockOrderDbAdapter = new Mock<IOrderDbAdapter>();
        _processOrders = new ProcessOrders(_mockOrderDbAdapter.Object);
    }

    [Fact]
    public async Task Execute_WithValidPendingToCalculated_ShouldProcessSuccessfully()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new ProcessOrdersInput
        {
            OrderIds = new List<Guid> { orderId },
            TargetStatus = OrderStatus.Calculated
        };

        var order = CreateTestOrder(orderId, OrderStatus.Pending);

        _mockOrderDbAdapter
            .Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<Order> { order });

        _mockOrderDbAdapter
            .Setup(x => x.Save(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _processOrders.Execute(input);

        // Assert
        result.Should().NotBeNull();
        result.TotalProcessed.Should().Be(1);
        result.SuccessCount.Should().Be(1);
        result.ErrorCount.Should().Be(0);
        result.ProcessedOrders.Should().HaveCount(1);
        result.ProcessedOrders[0].PreviousStatus.Should().Be(OrderStatus.Pending);
        result.ProcessedOrders[0].NewStatus.Should().Be(OrderStatus.Calculated);

        _mockOrderDbAdapter.Verify(x => x.GetByIds(It.IsAny<List<Guid>>()), Times.Once);
        _mockOrderDbAdapter.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithOrderNotFound_ShouldReturnError()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new ProcessOrdersInput
        {
            OrderIds = new List<Guid> { orderId },
            TargetStatus = OrderStatus.Calculated
        };

        _mockOrderDbAdapter
            .Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<Order>()); // Lista vazia

        // Act
        var result = await _processOrders.Execute(input);

        // Assert
        result.SuccessCount.Should().Be(0);
        result.ErrorCount.Should().Be(1);
        result.Errors.Should().ContainSingle();
        result.Errors[0].Should().Contain("não encontrado");
    }

    [Fact]
    public async Task Execute_WithInvalidTransition_ShouldReturnError()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new ProcessOrdersInput
        {
            OrderIds = new List<Guid> { orderId },
            TargetStatus = OrderStatus.Sent
        };

        var order = CreateTestOrder(orderId, OrderStatus.Pending); // Pending não pode ir para Sent

        _mockOrderDbAdapter
            .Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<Order> { order });

        // Act
        var result = await _processOrders.Execute(input);

        // Assert
        result.SuccessCount.Should().Be(0);
        result.ErrorCount.Should().Be(1);
        result.Errors.Should().ContainSingle();
        result.Errors[0].Should().Contain("não permite mudança para Sent");
    }

    [Fact]
    public async Task Execute_WithMultipleOrders_ShouldProcessAllValid()
    {
        // Arrange
        var orderId1 = Guid.NewGuid();
        var orderId2 = Guid.NewGuid();
        var orderId3 = Guid.NewGuid();

        var input = new ProcessOrdersInput
        {
            OrderIds = new List<Guid> { orderId1, orderId2, orderId3 },
            TargetStatus = OrderStatus.Calculated
        };

        var order1 = CreateTestOrder(orderId1, OrderStatus.Pending);
        var order2 = CreateTestOrder(orderId2, OrderStatus.Pending);
        var order3 = CreateTestOrder(orderId3, OrderStatus.Sent); // Inválido

        _mockOrderDbAdapter
            .Setup(x => x.GetByIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<Order> { order1, order2, order3 });

        _mockOrderDbAdapter
            .Setup(x => x.Save(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _processOrders.Execute(input);

        // Assert
        result.TotalProcessed.Should().Be(3);
        result.SuccessCount.Should().Be(2);
        result.ErrorCount.Should().Be(1);
        result.ProcessedOrders.Should().HaveCount(2);
        result.Errors.Should().HaveCount(1);
    }

    private static Order CreateTestOrder(Guid id, OrderStatus status)
    {
        return new Order
        {
            Id = id,
            OrderNumber = "ORD-001",
            ExternalOrderId = "EXT-001",
            Status = status,
            TotalAmount = 0,
            CreatedAt = DateTimeOffset.UtcNow,
            Items = new List<OrderItem>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    ProductCode = "PROD-001",
                    ProductName = "Produto A",
                    Quantity = 2,
                    UnitPrice = 25.50m,
                    TotalPrice = 51.00m,
                    CreatedAt = DateTimeOffset.UtcNow
                }
            }
        };
    }
}