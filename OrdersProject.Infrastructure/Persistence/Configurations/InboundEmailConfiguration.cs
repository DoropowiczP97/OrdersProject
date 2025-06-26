using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersProject.Domain.Entities;

public class InboundEmailConfiguration : IEntityTypeConfiguration<InboundEmail>
{
    public void Configure(EntityTypeBuilder<InboundEmail> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.From).HasMaxLength(512);
        builder.Property(x => x.Subject).HasMaxLength(1024);
        builder.Property(x => x.ReceivedAt).IsRequired();
        builder.Property(x => x.RawContent).IsRequired().HasColumnType("LONGBLOB");
    }
}
