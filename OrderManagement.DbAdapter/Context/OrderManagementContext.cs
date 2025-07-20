using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Pedidos.Models;
using OrderManagement.Domain.Security.Models;

namespace OrderManagement.DbAdapter.Context;

public class OrderManagementContext(
    DbContextOptions<OrderManagementContext> options
) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(OrderManagementContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }

}