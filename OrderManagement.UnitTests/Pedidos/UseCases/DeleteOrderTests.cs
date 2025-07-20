using FluentAssertions;
using Moq;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Implementations;
using OrderManagement.Domain.Pedidos.Exceptions;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.UnitTests.Pedidos.UseCases;

public class DeleteOrderTests
{
    private readonly Mock<IOrderDbAdapter> _mockOrderDbAdapter;
    private readonly DeleteOrder _deleteOrder;

    public DeleteOrderTests()
    {
        _mockOrderDbAdapter = new Mock<IOrderDbAdapter>();
        _deleteOrder = new DeleteOrder(_mockOrderDbAdapter.Object);
    }

    [Fact]
    public async Task Execute_WithValidOrderId_ShouldDeleteOrderSuccessfully()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        _mockOrderDbAdapter
            .Setup(x => x.Delete(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        await _deleteOrder.Execute(orderId);

        // Assert
        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
        _mockOrderDbAdapter.Verify(x => x.Delete(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithOrderNotFound_ShouldThrowException()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Order?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<OrderNotFoundException>(
            () => _deleteOrder.Execute(orderId));

        exception.Should().NotBeNull();

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
        _mockOrderDbAdapter.Verify(x => x.Delete(It.IsAny<Order>()), Times.Never);
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