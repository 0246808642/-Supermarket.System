using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Supermercado.Domain.Entities;
using Supermercado.Infrastructure.Identity;
using System.Reflection;

namespace Supermercado.Infrastructure.Data;

public class SupermercadoDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public SupermercadoDbContext(DbContextOptions<SupermercadoDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
