using Supermercado.Domain.Core;

namespace Supermercado.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Value { get; private set; }

    protected Money() { } // EF Core requires a parameterless constructor

    public Money(decimal value)
    {
        if (value < 0)
            throw new DomainException("O valor monetário não pode ser negativo.");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator decimal(Money money) => money.Value;
    public static implicit operator Money(decimal value) => new Money(value);
    
    public override string ToString() => $"$ {Value:N2}";
}
