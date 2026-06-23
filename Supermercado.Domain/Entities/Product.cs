using Supermercado.Domain.Core;
using Supermercado.Domain.ValueObjects;

namespace Supermercado.Domain.Entities;

public class Product : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Barcode Barcode { get; private set; }
    public Money Price { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }
    
    public DateTime ExpirationDate { get; private set; }
    public decimal ExpirationDiscountPercentage { get; private set; }
    
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }

    // Protected constructor for EF Core
    protected Product() { }

    public Product(string name, string description, Barcode barcode, Money price, Guid categoryId, DateTime expirationDate, decimal expirationDiscountPercentage, DateTime currentDate)
    {
        Validate(name, description);
        if ((expirationDate - currentDate).TotalDays < 5)
            throw new DomainException("A data de vencimento deve ser no mínimo 5 dias no futuro.");
        if (expirationDiscountPercentage < 0 || expirationDiscountPercentage > 90)
            throw new DomainException("A porcentagem de desconto deve estar entre 0 e 90.");

        Name = name;
        Description = description;
        Barcode = barcode;
        Price = price;
        StockQuantity = 0;
        IsActive = true;
        CategoryId = categoryId;
        ExpirationDate = expirationDate;
        ExpirationDiscountPercentage = expirationDiscountPercentage;
    }

    public void UpdateDetails(string name, string description, Guid categoryId)
    {
        Validate(name, description);
        Name = name;
        Description = description;
        CategoryId = categoryId;
    }

    public void UpdatePrice(Money newPrice)
    {
        if (newPrice.Value <= 0)
            throw new DomainException("O preço do produto deve ser maior que zero.");

        Price = newPrice;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("A quantidade a ser adicionada deve ser maior que zero.");

        StockQuantity += quantity;
    }

    public void RemoveStock(int quantity, DateTime currentDate)
    {
        if (!IsAvailableForSale(currentDate))
            throw new DomainException("Produto indisponível para venda (vencido ou sem estoque).");

        if (quantity <= 0)
            throw new DomainException("A quantidade a ser debitada deve ser maior que zero.");

        if (StockQuantity < quantity)
            throw new DomainException("Estoque insuficiente para esta operação.");

        StockQuantity -= quantity;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void UpdateDiscountPercentage(decimal percentage)
    {
        if (percentage < 0 || percentage > 90)
            throw new DomainException("A porcentagem de desconto deve estar entre 0 e 90.");
        ExpirationDiscountPercentage = percentage;
    }

    public Money GetDiscountedPrice()
    {
        var discount = Price.Value * (ExpirationDiscountPercentage / 100m);
        return new Money(Price.Value - discount);
    }

    public Money GetCurrentPrice(DateTime currentDate)
    {
        var daysToExpiration = (ExpirationDate - currentDate).TotalDays;
        if (daysToExpiration >= 0 && daysToExpiration <= 10)
        {
            return GetDiscountedPrice();
        }
        return Price;
    }

    public bool IsAvailableForSale(DateTime currentDate)
    {
        if (currentDate > ExpirationDate) return false;
        return StockQuantity > 0;
    }

    private void Validate(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("O nome do produto não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("A descrição do produto não pode ser vazia.");
    }
}
