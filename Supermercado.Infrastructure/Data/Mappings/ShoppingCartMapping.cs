using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Supermercado.Domain.Entities;
namespace Supermercado.Infrastructure.Data.Mappings;
public class ShoppingCartMapping : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.CustomerId).IsRequired();
        builder.HasMany(c => c.Items).WithOne(i => i.ShoppingCart).HasForeignKey(i => i.ShoppingCartId).OnDelete(DeleteBehavior.Cascade);
    }
}
