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
    
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }

    // Protected constructor for EF Core
    protected Product() { }

    public Product(string name, string description, Barcode barcode, Money price, Guid categoryId)
    {
        Validate(name, description);

        Name = name;
        Description = description;
        Barcode = barcode;
        Price = price;
        StockQuantity = 0;
        IsActive = true;
        CategoryId = categoryId;
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

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("A quantidade a ser debitada deve ser maior que zero.");

        if (StockQuantity < quantity)
            throw new DomainException("Estoque insuficiente para esta operação.");

        StockQuantity -= quantity;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    private void Validate(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("O nome do produto não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("A descrição do produto não pode ser vazia.");
    }
}
