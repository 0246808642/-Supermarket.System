using Supermercado.Domain.Entities;

namespace Supermercado.Domain.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
}
