using Supermercado.Domain.Core;

namespace Supermercado.Domain.Entities;

public class Address : Entity
{
    public Guid CustomerId { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }

    protected Address() { }
    
    public Address(Guid customerId, string street, string number, string city, string state, string zipCode)
    {
        CustomerId = customerId;
        Street = street;
        Number = number;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
}
