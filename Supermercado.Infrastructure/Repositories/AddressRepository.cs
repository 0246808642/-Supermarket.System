using Microsoft.EntityFrameworkCore;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;
using Supermercado.Infrastructure.Data;
namespace Supermercado.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly SupermercadoDbContext _context;
    public AddressRepository(SupermercadoDbContext context) => _context = context;

    public async Task<IEnumerable<Address>> GetByCustomerIdAsync(Guid customerId) =>
        await _context.Addresses.Where(a => a.CustomerId == customerId).ToListAsync();

    public async Task<Address?> GetByIdAsync(Guid id) =>
        await _context.Addresses.FindAsync(id);

    public void Add(Address address) => _context.Addresses.Add(address);
    public void Remove(Address address) => _context.Addresses.Remove(address);
}
