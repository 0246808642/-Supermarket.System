using Supermercado.Domain.Entities;
namespace Supermercado.Domain.Interfaces;
public interface IAddressRepository
{
    Task<IEnumerable<Address>> GetByCustomerIdAsync(Guid customerId);
    Task<Address?> GetByIdAsync(Guid id);
    void Add(Address address);
    void Remove(Address address);
}
