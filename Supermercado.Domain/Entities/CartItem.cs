using Supermercado.Domain.Core;

namespace Supermercado.Domain.Entities;

public class CartItem : Entity
{
    public Guid ShoppingCartId { get; private set; }
    public ShoppingCart ShoppingCart { get; private set; }
    
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    
    public int Quantity { get; private set; }

    protected CartItem() { }

    internal CartItem(Guid shoppingCartId, Guid productId, int quantity)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
    }

    internal void UpdateQuantity(int quantity)
    {
        if (quantity <= 0) throw new DomainException("Quantidade deve ser maior que zero.");
        Quantity = quantity;
    }
}
