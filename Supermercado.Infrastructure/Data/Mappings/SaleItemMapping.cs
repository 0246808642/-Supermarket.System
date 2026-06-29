using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Supermercado.Domain.Entities;

namespace Supermercado.Infrastructure.Data.Mappings;

public class SaleItemMapping : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(si => si.Id);

        builder.Property(si => si.Quantity)
            .IsRequired();

        builder.OwnsOne(si => si.UnitPrice, m =>
        {
            m.Property(p => p.Value)
             .HasColumnName("UnitPrice")
             .HasPrecision(18, 2)
             .IsRequired();
        });

        builder.OwnsOne(si => si.TotalPrice, m =>
        {
            m.Property(p => p.Value)
             .HasColumnName("TotalPrice")
             .HasPrecision(18, 2)
             .IsRequired();
        });

        builder.HasOne(si => si.Product)
            .WithMany()
            .HasForeignKey(si => si.ProductId);
    }
}
