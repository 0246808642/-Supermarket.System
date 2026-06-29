using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Supermercado.Domain.Entities;

namespace Supermercado.Infrastructure.Data.Mappings;

public class PaymentMapping : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Method).IsRequired();
        builder.Property(p => p.PaymentDate).IsRequired();

        builder.OwnsOne(p => p.Amount, m =>
        {
            m.Property(a => a.Value)
             .HasColumnName("Amount")
             .HasPrecision(18, 2)
             .IsRequired();
        });

        builder.HasOne(p => p.Sale)
            .WithMany(s => s.Payments)
            .HasForeignKey(p => p.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
