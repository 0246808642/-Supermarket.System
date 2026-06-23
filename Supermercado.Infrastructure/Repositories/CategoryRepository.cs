using Microsoft.EntityFrameworkCore;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;
using Supermercado.Infrastructure.Data;

namespace Supermercado.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly SupermercadoDbContext _context;

    public CategoryRepository(SupermercadoDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await Task.CompletedTask;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }
}
