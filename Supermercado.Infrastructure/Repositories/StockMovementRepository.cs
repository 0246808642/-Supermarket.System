using Microsoft.EntityFrameworkCore;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;
using Supermercado.Infrastructure.Data;

namespace Supermercado.Infrastructure.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly SupermercadoDbContext _context;

    public StockMovementRepository(SupermercadoDbContext context)
    {
        _context = context;
    }

    public void Add(StockMovement movement)
    {
        _context.StockMovements.Add(movement);
    }

    public async Task<IEnumerable<StockMovement>> GetByProductIdAsync(Guid productId)
    {
        return await _context.StockMovements
            .AsNoTracking()
            .Where(sm => sm.ProductId == productId)
            .OrderByDescending(sm => sm.MovementDate)
            .ToListAsync();
    }
}
