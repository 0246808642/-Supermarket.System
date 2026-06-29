using Microsoft.EntityFrameworkCore;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;
using Supermercado.Infrastructure.Data;
namespace Supermercado.Infrastructure.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly SupermercadoDbContext _context;
    public ShoppingCartRepository(SupermercadoDbContext context) => _context = context;

    public async Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId) =>
        await _context.ShoppingCarts.Include(c => c.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

    public void Add(ShoppingCart cart) => _context.ShoppingCarts.Add(cart);
    public void Update(ShoppingCart cart) => _context.ShoppingCarts.Update(cart);
}
