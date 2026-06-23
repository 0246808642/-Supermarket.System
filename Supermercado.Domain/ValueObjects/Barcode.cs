using Supermercado.Domain.Core;

namespace Supermercado.Domain.ValueObjects;

public class Barcode : ValueObject
{
    public string Code { get; private set; }

    protected Barcode() { } // EF Core requires a parameterless constructor

    public Barcode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("O código de barras não pode ser vazio.");

        if (code.Length < 8 || code.Length > 14)
            throw new DomainException("O código de barras deve ter entre 8 e 14 caracteres.");

        Code = code;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
    
    public override string ToString() => Code;
}
