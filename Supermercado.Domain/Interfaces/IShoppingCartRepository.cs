using Supermercado.Domain.Entities;
namespace Supermercado.Domain.Interfaces;
public interface IShoppingCartRepository
{
    Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId);
    void Add(ShoppingCart cart);
    void Update(ShoppingCart cart);
}
