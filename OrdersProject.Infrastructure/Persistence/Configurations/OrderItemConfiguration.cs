using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersProject.Domain.Entities;

namespace OrdersProject.Infrastructure.Persistence.Configurations;
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnType("char(36)");

        builder.Property(x => x.ProductName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.Quantity)
               .IsRequired();

        builder.Property(x => x.Price)
               .HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Order)
               .WithMany(o => o.Items)
               .HasForeignKey(x => x.OrderId);
    }
}
