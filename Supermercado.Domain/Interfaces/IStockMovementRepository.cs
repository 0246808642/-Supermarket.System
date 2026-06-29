using Supermercado.Domain.Entities;

namespace Supermercado.Domain.Interfaces;

public interface IStockMovementRepository
{
    void Add(StockMovement movement);
    Task<IEnumerable<StockMovement>> GetByProductIdAsync(Guid productId);
}
