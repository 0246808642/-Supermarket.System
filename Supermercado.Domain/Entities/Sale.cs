using Supermercado.Domain.Core;
using Supermercado.Domain.ValueObjects;

namespace Supermercado.Domain.Entities;

public class Sale : Entity, IAggregateRoot
{
    public DateTime SaleDate { get; private set; }
    public Money TotalAmount { get; private set; }
    public Guid CashierId { get; private set; }
    
    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    protected Sale() { }

    public Sale(Guid cashierId, DateTime saleDate)
    {
        CashierId = cashierId;
        SaleDate = saleDate;
        TotalAmount = new Money(0);
    }

    public void AddItem(Product product, int quantity, DateTime currentDate)
    {
        if (quantity <= 0) throw new DomainException("Quantidade deve ser maior que zero.");
        if (!product.IsAvailableForSale(currentDate)) throw new DomainException($"Produto {product.Name} não está disponível para venda.");

        var currentPrice = product.GetCurrentPrice(currentDate);
        var saleItem = new SaleItem(Id, product.Id, quantity, currentPrice);
        _items.Add(saleItem);
        
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        decimal total = _items.Sum(i => i.TotalPrice.Value);
        TotalAmount = new Money(total);
    }
}
