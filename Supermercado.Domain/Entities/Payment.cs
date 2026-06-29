using Supermercado.Domain.Core;
using Supermercado.Domain.Enums;
using Supermercado.Domain.ValueObjects;

namespace Supermercado.Domain.Entities;

public class Payment : Entity
{
    public Guid SaleId { get; private set; }
    public Sale Sale { get; private set; }
    public PaymentMethod Method { get; private set; }
    public Money Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }

    protected Payment() { }

    internal Payment(Guid saleId, PaymentMethod method, decimal amount)
    {
        SaleId = saleId;
        Method = method;
        Amount = new Money(amount);
        PaymentDate = DateTime.UtcNow;
    }
}
