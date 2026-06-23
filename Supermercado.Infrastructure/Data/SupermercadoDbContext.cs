using Microsoft.EntityFrameworkCore;
using Supermercado.Domain.Entities;
using System.Reflection;

namespace Supermercado.Infrastructure.Data;

public class SupermercadoDbContext : DbContext
{
    public SupermercadoDbContext(DbContextOptions<SupermercadoDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        base.OnModelCreating(modelBuilder);
    }
}
