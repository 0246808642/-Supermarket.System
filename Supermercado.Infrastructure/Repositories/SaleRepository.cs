using Microsoft.EntityFrameworkCore;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;
using Supermercado.Infrastructure.Data;

namespace Supermercado.Infrastructure.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly SupermercadoDbContext _context;

    public SaleRepository(SupermercadoDbContext context)
    {
        _context = context;
    }

    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales
            .Include(s => s.Items)
            .AsNoTracking()
            .ToListAsync();
    }

    public void Add(Sale sale)
    {
        _context.Sales.Add(sale);
    }
}
