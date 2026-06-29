using Supermercado.Domain.Core;
using Supermercado.Domain.ValueObjects;

namespace Supermercado.Domain.Entities;

public class SaleItem : Entity
{
    public Guid SaleId { get; private set; }
    public Sale Sale { get; private set; }
    
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money TotalPrice { get; private set; }

    protected SaleItem() { }

    internal SaleItem(Guid saleId, Guid productId, int quantity, Money unitPrice)
    {
        SaleId = saleId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalPrice = new Money(unitPrice.Value * quantity);
    }
}
