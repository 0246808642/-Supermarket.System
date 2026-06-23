using Supermercado.Domain.Interfaces;

namespace Supermercado.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly SupermercadoDbContext _context;

    public UnitOfWork(SupermercadoDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CommitAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
