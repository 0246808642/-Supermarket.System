using Supermercado.Domain.Entities;

namespace Supermercado.Domain.Interfaces;

public interface ISaleRepository
{
    Task<Sale?> GetByIdAsync(Guid id);
    Task<IEnumerable<Sale>> GetAllAsync();
    void Add(Sale sale);
}
