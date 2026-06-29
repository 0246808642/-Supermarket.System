using Supermercado.Domain.Core;

namespace Supermercado.Domain.Entities;

public class ShoppingCart : Entity, IAggregateRoot
{
    public Guid CustomerId { get; private set; }
    
    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    protected ShoppingCart() { }
    
    public ShoppingCart(Guid customerId)
    {
        CustomerId = customerId;
    }

    public void AddItem(Guid productId, int quantity)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            _items.Add(new CartItem(Id, productId, quantity));
        }
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
            _items.Remove(item);
    }
    
    public void Clear()
    {
        _items.Clear();
    }
}
