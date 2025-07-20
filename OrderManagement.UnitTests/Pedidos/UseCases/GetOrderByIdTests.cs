using FluentAssertions;
using Moq;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Implementations;
using OrderManagement.Domain.Pedidos.Exceptions;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.UnitTests.Pedidos.UseCases;

public class GetOrderByIdTests
{
    private readonly Mock<IOrderDbAdapter> _mockOrderDbAdapter;
    private readonly GetOrderById _getOrderById;

    public GetOrderByIdTests()
    {
        _mockOrderDbAdapter = new Mock<IOrderDbAdapter>();
        _getOrderById = new GetOrderById(_mockOrderDbAdapter.Object);
    }

    [Fact]
    public async Task Execute_WithValidOrderId_ShouldReturnOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        // Act
        var result = await _getOrderById.Execute(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(orderId);
        result.OrderNumber.Should().Be("ORD-001");
        result.ExternalOrderId.Should().Be("EXT-001");
        result.Status.Should().Be(OrderStatus.Pending);
        result.TotalAmount.Should().Be(0);
        result.Items.Should().HaveCount(1);
        result.Items[0].ProductCode.Should().Be("PROD-001");
        result.Items[0].TotalPrice.Should().Be(51.00m);

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
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
            () => _getOrderById.Execute(orderId));

        exception.Should().NotBeNull();

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
    }

    [Fact]
    public async Task Execute_WithCalculatedOrder_ShouldReturnOrderWithTotalAmount()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Calculated);
        order.TotalAmount = 85.50m;
        order.CalculatedAt = DateTimeOffset.UtcNow;

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        // Act
        var result = await _getOrderById.Execute(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(OrderStatus.Calculated);
        result.TotalAmount.Should().Be(85.50m);
        result.CalculatedAt.Should().NotBeNull();

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
    }

    [Fact]
    public async Task Execute_WithOrderWithMultipleItems_ShouldReturnAllItems()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = CreateTestOrder(orderId, OrderStatus.Pending);
        order.Items.Add(new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductCode = "PROD-002",
            ProductName = "Produto B",
            Quantity = 1,
            UnitPrice = 34.50m,
            TotalPrice = 34.50m,
            CreatedAt = DateTimeOffset.UtcNow
        });

        _mockOrderDbAdapter
            .Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        // Act
        var result = await _getOrderById.Execute(orderId);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items[0].ProductCode.Should().Be("PROD-001");
        result.Items[1].ProductCode.Should().Be("PROD-002");

        _mockOrderDbAdapter.Verify(x => x.GetById(orderId), Times.Once);
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