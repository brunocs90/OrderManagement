using OrderManagement.Domain.Pedidos.Models;
using OrderManagement.Domain.Security.Models;
using OrderManagement.Shared.Extensions;

namespace OrderManagement.DbAdapter.Context;

public static class DbContextExtensions
{
    public static void EnsureSeedData(this OrderManagementContext context)
    {
        if (context.Users.Any())
            return;

        var usuarioAdminId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var orderId = Guid.Parse("c3c3c3c3-c3c3-c3c3-c3c3-c3c3c3c3c3c3");
        var orderItem1Id = Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4");
        var orderItem2Id = Guid.Parse("e5e5e5e5-e5e5-e5e5-e5e5-e5e5e5e5e5e5");
        var orderItem3Id = Guid.Parse("f6f6f6f6-f6f6-f6f6-f6f6-f6f6f6f6f6f6");

        // Admin User
        if (!context.Users.Any(x => x.Id == usuarioAdminId))
        {
            context.Users.Add(new User
            {
                Id = usuarioAdminId,
                Login = "admin",
                Password = "admin".Encrypt(),
                Name = "Administrator",
                Email = "admin@example.com",
                Active = true,
                CreatedAt = DateTimeOffset.UtcNow
            });
            context.SaveChanges();
        }



        // Order e OrderItems
        if (!context.Orders.Any())
        {
            context.Orders.Add(new Order
            {
                Id = orderId,
                OrderNumber = "ORD-001",
                ExternalOrderId = "EXT-001",
                Status = OrderStatus.Calculated,
                TotalAmount = 85.50m,
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-1),
                CalculatedAt = DateTimeOffset.UtcNow.AddHours(-2)
            });

            context.SaveChanges();

            // OrderItems
            context.OrderItems.Add(new OrderItem
            {
                Id = orderItem1Id,
                OrderId = orderId,
                ProductCode = "PROD-001",
                ProductName = "Produto Teste 1",
                Quantity = 2,
                UnitPrice = 25.00m,
                TotalPrice = 50.00m,
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-1)
            });

            context.OrderItems.Add(new OrderItem
            {
                Id = orderItem2Id,
                OrderId = orderId,
                ProductCode = "PROD-002",
                ProductName = "Produto Teste 2",
                Quantity = 1,
                UnitPrice = 35.50m,
                TotalPrice = 35.50m,
                CreatedAt = DateTimeOffset.UtcNow.AddDays(-1)
            });
        }

        // Order Pendente para teste
        if (!context.Orders.Any(x => x.ExternalOrderId == "EXT-002"))
        {
            var orderPendenteId = Guid.Parse("a7a7a7a7-a7a7-a7a7-a7a7-a7a7a7a7a7a7");

            context.Orders.Add(new Order
            {
                Id = orderPendenteId,
                OrderNumber = "ORD-002",
                ExternalOrderId = "EXT-002",
                Status = OrderStatus.Pending,
                TotalAmount = 120.00m,
                CreatedAt = DateTimeOffset.UtcNow.AddHours(-1)
            });

            context.SaveChanges();

            context.OrderItems.Add(new OrderItem
            {
                Id = orderItem3Id,
                OrderId = orderPendenteId,
                ProductCode = "PROD-003",
                ProductName = "Produto Pendente",
                Quantity = 3,
                UnitPrice = 40.00m,
                TotalPrice = 120.00m,
                CreatedAt = DateTimeOffset.UtcNow.AddHours(-1)
            });
        }

        context.SaveChanges();
    }
}
