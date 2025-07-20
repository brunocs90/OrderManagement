using FluentAssertions;
using Moq;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Implementations;
using OrderManagement.Domain.Pedidos.Exceptions;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.UnitTests.Pedidos.UseCases;

public class CreateOrderTests
{
    private readonly Mock<IOrderDbAdapter> _mockOrderDbAdapter;
    private readonly CreateOrder _createOrder;

    public CreateOrderTests()
    {
        _mockOrderDbAdapter = new Mock<IOrderDbAdapter>();
        _createOrder = new CreateOrder(_mockOrderDbAdapter.Object);
    }

    [Fact]
    public async Task Execute_WithValidInput_ShouldCreateOrderSuccessfully()
    {
        // Arrange
        var input = new CreateOrderInput
        {
            OrderNumber = "ORD-001",
            ExternalOrderId = "EXT-001",
            Items = new List<CreateOrderItemInput>
            {
                new()
                {
                    ProductCode = "PROD-001",
                    ProductName = "Produto A",
                    Quantity = 2,
                    UnitPrice = 25.50m
                }
            }
        };

        _mockOrderDbAdapter
            .Setup(x => x.ExistsByExternalOrderId(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockOrderDbAdapter
            .Setup(x => x.Save(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _createOrder.Execute(input);

        // Assert
        result.Should().NotBeNull();
        result.OrderNumber.Should().Be("ORD-001");
        result.ExternalOrderId.Should().Be("EXT-001");
        result.Status.Should().Be(OrderStatus.Pending);
        result.TotalAmount.Should().Be(0); // Não calcula na criação
        result.Items.Should().HaveCount(1);
        result.Items[0].ProductCode.Should().Be("PROD-001");
        result.Items[0].TotalPrice.Should().Be(51.00m);

        _mockOrderDbAdapter.Verify(x => x.ExistsByExternalOrderId("EXT-001"), Times.Once);
        _mockOrderDbAdapter.Verify(x => x.Save(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithMultipleItems_ShouldCreateOrderWithAllItems()
    {
        // Arrange
        var input = new CreateOrderInput
        {
            OrderNumber = "ORD-002",
            ExternalOrderId = "EXT-002",
            Items = new List<CreateOrderItemInput>
            {
                new()
                {
                    ProductCode = "PROD-001",
                    ProductName = "Produto A",
                    Quantity = 2,
                    UnitPrice = 25.50m
                },
                new()
                {
                    ProductCode = "PROD-002",
                    ProductName = "Produto B",
                    Quantity = 1,
                    UnitPrice = 34.50m
                }
            }
        };

        _mockOrderDbAdapter
            .Setup(x => x.ExistsByExternalOrderId(It.IsAny<string>()))
            .ReturnsAsync(false);

        _mockOrderDbAdapter
            .Setup(x => x.Save(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _createOrder.Execute(input);

        // Assert
        result.Items.Should().HaveCount(2);
        result.Items[0].TotalPrice.Should().Be(51.00m);
        result.Items[1].TotalPrice.Should().Be(34.50m);
        result.TotalAmount.Should().Be(0); // Não calcula na criação
    }

    [Fact]
    public async Task Execute_WithDuplicateExternalOrderId_ShouldThrowException()
    {
        // Arrange
        var input = new CreateOrderInput
        {
            OrderNumber = "ORD-001",
            ExternalOrderId = "EXT-001",
            Items = new List<CreateOrderItemInput>
            {
                new()
                {
                    ProductCode = "PROD-001",
                    ProductName = "Produto A",
                    Quantity = 1,
                    UnitPrice = 10.00m
                }
            }
        };

        _mockOrderDbAdapter
            .Setup(x => x.ExistsByExternalOrderId(It.IsAny<string>()))
            .ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<CreateOrderException>(
            () => _createOrder.Execute(input));

        exception.Should().NotBeNull();
    }
}