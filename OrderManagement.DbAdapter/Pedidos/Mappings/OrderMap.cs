using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderManagement.Domain.Pedidos.Models;

namespace OrderManagement.DbAdapter.Pedidos.Mappings;

public class OrderMap : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()").ValueGeneratedOnAdd();

        builder.Property(x => x.OrderNumber).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ExternalOrderId).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().IsRequired();
        builder.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("GETUTCDATE()");
        builder.Property(x => x.DeletedAt);
        builder.Property(x => x.CalculatedAt);
        builder.Property(x => x.SentAt);

        builder.HasQueryFilter(x => x.DeletedAt == null);

        builder.HasIndex(x => x.DeletedAt).HasFilter("[DeletedAt] IS NULL").HasDatabaseName("IX_Order_DeletedAt");
        builder.HasIndex(x => x.ExternalOrderId).HasDatabaseName("IX_Order_ExternalOrderId");
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_Order_Status");
        builder.HasIndex(x => x.CreatedAt).HasDatabaseName("IX_Order_CreatedAt");
    }
}