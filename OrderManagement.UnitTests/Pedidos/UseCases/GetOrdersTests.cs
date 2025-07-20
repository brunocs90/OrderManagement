using FluentAssertions;
using Moq;
using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Implementations;
using OrderManagement.Domain.Pedidos.Filters;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.UnitTests.Pedidos.UseCases;

public class GetOrdersTests
{
    private readonly Mock<IOrderDbAdapter> _mockOrderDbAdapter;
    private readonly GetOrders _getOrders;

    public GetOrdersTests()
    {
        _mockOrderDbAdapter = new Mock<IOrderDbAdapter>();
        _getOrders = new GetOrders(_mockOrderDbAdapter.Object);
    }

    [Fact]
    public async Task Execute_WithValidInput_ShouldReturnOrdersSuccessfully()
    {
        // Arrange
        var input = new GetOrdersInput
        {
            PageNumber = 1,
            PageSize = 10
        };

        var orders = CreateTestOrders();
        var pagedList = new PagedListDto<Order>
        {
            Items = orders,
            TotalCount = orders.Count,
            PageNumber = 1,
            PageSize = 10
        };

        _mockOrderDbAdapter
            .Setup(x => x.Get(It.IsAny<OrderFilter>()))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getOrders.Execute(input);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);

        _mockOrderDbAdapter.Verify(x => x.Get(It.IsAny<OrderFilter>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithEmptyResult_ShouldReturnEmptyList()
    {
        // Arrange
        var input = new GetOrdersInput
        {
            PageNumber = 1,
            PageSize = 10
        };

        var emptyPagedList = new PagedListDto<Order>
        {
            Items = new List<Order>(),
            TotalCount = 0,
            PageNumber = 1,
            PageSize = 10
        };

        _mockOrderDbAdapter
            .Setup(x => x.Get(It.IsAny<OrderFilter>()))
            .ReturnsAsync(emptyPagedList);

        // Act
        var result = await _getOrders.Execute(input);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Execute_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var input = new GetOrdersInput
        {
            PageNumber = 2,
            PageSize = 5
        };

        var orders = CreateTestOrders();
        var pagedList = new PagedListDto<Order>
        {
            Items = orders.Take(5).ToList(),
            TotalCount = 10,
            PageNumber = 2,
            PageSize = 5
        };

        _mockOrderDbAdapter
            .Setup(x => x.Get(It.IsAny<OrderFilter>()))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getOrders.Execute(input);

        // Assert
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(10);
    }

    [Fact]
    public async Task Execute_WithFilterByOrderNumber_ShouldApplyFilter()
    {
        // Arrange
        var input = new GetOrdersInput
        {
            PageNumber = 1,
            PageSize = 10,
            OrderNumber = "ORD-001"
        };

        var orders = CreateTestOrders().Where(o => o.OrderNumber == "ORD-001").ToList();
        var pagedList = new PagedListDto<Order>
        {
            Items = orders,
            TotalCount = orders.Count,
            PageNumber = 1,
            PageSize = 10
        };

        _mockOrderDbAdapter
            .Setup(x => x.Get(It.IsAny<OrderFilter>()))
            .ReturnsAsync(pagedList);

        // Act
        var result = await _getOrders.Execute(input);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items[0].OrderNumber.Should().Be("ORD-001");
    }

    private static List<Order> CreateTestOrders()
    {
        return new List<Order>
        {
            CreateTestOrder(Guid.NewGuid(), OrderStatus.Pending, "ORD-001", "EXT-001"),
            CreateTestOrder(Guid.NewGuid(), OrderStatus.Calculated, "ORD-002", "EXT-002")
        };
    }

    private static Order CreateTestOrder(Guid id, OrderStatus status, string orderNumber, string externalOrderId)
    {
        return new Order
        {
            Id = id,
            OrderNumber = orderNumber,
            ExternalOrderId = externalOrderId,
            Status = status,
            TotalAmount = status == OrderStatus.Pending ? 0 : 85.50m,
            CreatedAt = DateTimeOffset.UtcNow,
            CalculatedAt = status == OrderStatus.Calculated ? DateTimeOffset.UtcNow : null,
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