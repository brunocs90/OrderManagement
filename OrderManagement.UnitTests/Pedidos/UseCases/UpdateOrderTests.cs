using FluentAssertions;
using Moq;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Implementations;
using OrderManagement.Domain.Pedidos.Exceptions;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.UnitTests.Pedidos.UseCases;

public class UpdateOrderTests
{
    private readonly Mock<IOrderDbAdapter> _mockOrderDbAdapter;
    private readonly UpdateOrder _updateOrder;

    public UpdateOrderTests()
    {
        _mockOrderDbAdapter = new Mock<IOrderDbAdapter>();
        _updateOrder = new UpdateOrder(_mockOrderDbAdapter.Object);
    }

    [Fact]
    public async Task Execute_WithValidInput_ShouldUpdateOrderSuccessfully()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new UpdateOrderInput
        {
            Status = OrderStatus.Calculated
        };

        var existingOrder = CreateTestOrder(orderId, OrderStatus.Pending);

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(existingOrder);

        _mockOrderDbAdapter
            .Setup(x => x.Save(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _updateOrder.Execute(orderId, input);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(orderId);
        result.Status.Should().Be(OrderStatus.Calculated);
        result.CalculatedAt.Should().NotBeNull();

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
        _mockOrderDbAdapter.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithOrderNotFound_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new UpdateOrderInput
        {
            Status = OrderStatus.Calculated
        };

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<OrderNotFoundException>(
            () => _updateOrder.Execute(orderId, input));

        exception.Should().NotBeNull();

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
        _mockOrderDbAdapter.Verify(x => x.Save(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Execute_WithSentStatus_ShouldUpdateSuccessfully()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new UpdateOrderInput
        {
            Status = OrderStatus.Sent
        };

        var existingOrder = CreateTestOrder(orderId, OrderStatus.Calculated);
        existingOrder.CalculatedAt = DateTimeOffset.UtcNow;

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(existingOrder);

        _mockOrderDbAdapter
            .Setup(x => x.Save(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _updateOrder.Execute(orderId, input);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(OrderStatus.Sent);
        result.SentAt.Should().NotBeNull();

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
        _mockOrderDbAdapter.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithPendingStatus_ShouldUpdateSuccessfully()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var input = new UpdateOrderInput
        {
            Status = OrderStatus.Pending
        };

        var existingOrder = CreateTestOrder(orderId, OrderStatus.Calculated);

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(existingOrder);

        _mockOrderDbAdapter
            .Setup(x => x.Save(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _updateOrder.Execute(orderId, input);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(OrderStatus.Pending);

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
        _mockOrderDbAdapter.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
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