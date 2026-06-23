using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Supermercado.Domain.Entities;

namespace Supermercado.Infrastructure.Data.Mappings;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(p => p.Description)
            .IsRequired()
            .HasColumnType("varchar(250)");

        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.ExpirationDate)
            .IsRequired();

        builder.Property(p => p.ExpirationDiscountPercentage)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(p => p.CreatedByUserId)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.RemovedByUserId);

        builder.Property(p => p.RemovedAt);

        builder.Property(p => p.IsRemoved)
            .IsRequired();

        builder.OwnsOne(p => p.Barcode, cb =>
        {
            cb.Property(c => c.Code)
                .HasColumnName("Barcode")
                .HasColumnType("varchar(14)")
                .IsRequired();
        });

        builder.OwnsOne(p => p.Price, cb =>
        {
            cb.Property(c => c.Value)
                .HasColumnName("Price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
        });

        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId);
    }
}
