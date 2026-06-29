using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Supermercado.Domain.Entities;

namespace Supermercado.Infrastructure.Data.Mappings;

public class SaleMapping : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.SaleDate)
            .IsRequired();

        builder.Property(s => s.CashierId)
            .IsRequired(false);

        builder.Property(s => s.CustomerId)
            .IsRequired(false);

        builder.OwnsOne(s => s.TotalAmount, m =>
        {
            m.Property(p => p.Value)
             .HasColumnName("TotalAmount")
             .HasPrecision(18, 2)
             .IsRequired();
        });

        builder.HasMany(s => s.Items)
            .WithOne(i => i.Sale)
            .HasForeignKey(i => i.SaleId);
    }
}
