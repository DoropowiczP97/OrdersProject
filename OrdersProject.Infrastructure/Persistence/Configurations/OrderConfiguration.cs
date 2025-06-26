using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersProject.Domain.Entities;

namespace OrdersProject.Infrastructure.Persistence.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
               .HasColumnType("char(36)");

        builder.Property(x => x.SourceEmail)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.CustomerName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.OrderDate)
               .IsRequired();

        builder.Property(x => x.PaymentMethod)
               .HasMaxLength(128);

        builder.Property(x => x.ShippingAddress)
               .HasMaxLength(512);

        builder.Property(x => x.ShippingCost)
               .HasColumnType("decimal(18,2)");

        builder.Property(x => x.TotalAmount)
               .HasColumnType("decimal(18,2)");

        builder.HasMany(o => o.Items)
               .WithOne(i => i.Order)
               .HasForeignKey(i => i.OrderId);
    }
}
