using Supermercado.Domain.Core;

namespace Supermercado.Domain.Entities;

public class Category : Entity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation property
    public ICollection<Product> Products { get; private set; } = new List<Product>();

    // Protected constructor for EF Core
    protected Category() { }

    public Category(string name, string description)
    {
        Validate(name);

        Name = name;
        Description = description;
        IsActive = true;
    }

    public void UpdateDetails(string name, string description)
    {
        Validate(name);
        Name = name;
        Description = description;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    private void Validate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("O nome da categoria não pode ser vazio.");
    }
}
