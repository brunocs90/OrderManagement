using FluentAssertions;
using Moq;
using OrderManagement.Application.Commons.Dtos;
using OrderManagement.Application.Pedidos.Adapters;
using OrderManagement.Application.Pedidos.UseCases.Dtos;
using OrderManagement.Application.Pedidos.UseCases.Implementations;
using OrderManagement.Domain.Pedidos.Filters;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.UnitTests.Pedidos.UseCases;

public class GetOrdersByStatusTests
{
    private readonly Mock<IOrderDbAdapter> _mockOrderDbAdapter;
    private readonly GetOrdersByStatus _getOrdersByStatus;

    public GetOrdersByStatusTests()
    {
        _mockOrderDbAdapter = new Mock<IOrderDbAdapter>();
        _getOrdersByStatus = new GetOrdersByStatus(_mockOrderDbAdapter.Object);
    }

    [Fact]
    public async Task Execute_WithPendingStatus_ShouldReturnPendingOrders()
    {
        // Arrange
        var input = new GetOrdersByStatusInput
        {
            Status = OrderStatus.Pending,
            PageNumber = 1,
            PageSize = 10
        };

        var orders = CreateTestOrdersByStatus(OrderStatus.Pending);
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
        var result = await _getOrdersByStatus.Execute(input);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.Should().OnlyContain(o => o.Status == OrderStatus.Pending);

        _mockOrderDbAdapter.Verify(x => x.Get(It.IsAny<OrderFilter>()), Times.Once);
    }

    [Fact]
    public async Task Execute_WithCalculatedStatus_ShouldReturnCalculatedOrders()
    {
        // Arrange
        var input = new GetOrdersByStatusInput
        {
            Status = OrderStatus.Calculated,
            PageNumber = 1,
            PageSize = 10
        };

        var orders = CreateTestOrdersByStatus(OrderStatus.Calculated);
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
        var result = await _getOrdersByStatus.Execute(input);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Should().OnlyContain(o => o.Status == OrderStatus.Calculated);
        result.Items.Should().OnlyContain(o => o.TotalAmount > 0);
    }

    [Fact]
    public async Task Execute_WithSentStatus_ShouldReturnSentOrders()
    {
        // Arrange
        var input = new GetOrdersByStatusInput
        {
            Status = OrderStatus.Sent,
            PageNumber = 1,
            PageSize = 10
        };

        var orders = CreateTestOrdersByStatus(OrderStatus.Sent);
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
        var result = await _getOrdersByStatus.Execute(input);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Should().OnlyContain(o => o.Status == OrderStatus.Sent);
        result.Items.Should().OnlyContain(o => o.SentAt.HasValue);
    }

    [Fact]
    public async Task Execute_WithEmptyResult_ShouldReturnEmptyList()
    {
        // Arrange
        var input = new GetOrdersByStatusInput
        {
            Status = OrderStatus.Cancelled,
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
        var result = await _getOrdersByStatus.Execute(input);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task Execute_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var input = new GetOrdersByStatusInput
        {
            Status = OrderStatus.Pending,
            PageNumber = 2,
            PageSize = 5
        };

        var orders = CreateTestOrdersByStatus(OrderStatus.Pending);
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
        var result = await _getOrdersByStatus.Execute(input);

        // Assert
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(5);
        result.TotalCount.Should().Be(10);
    }

    private static List<Order> CreateTestOrdersByStatus(OrderStatus status)
    {
        return new List<Order>
        {
            CreateTestOrder(Guid.NewGuid(), status, $"ORD-{status}-001", $"EXT-{status}-001"),
            CreateTestOrder(Guid.NewGuid(), status, $"ORD-{status}-002", $"EXT-{status}-002")
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
            CalculatedAt = status == OrderStatus.Calculated || status == OrderStatus.Sent ? DateTimeOffset.UtcNow : null,
            SentAt = status == OrderStatus.Sent ? DateTimeOffset.UtcNow : null,
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