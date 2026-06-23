using Supermercado.Domain.Core;
using Supermercado.Domain.Entities;

namespace Supermercado.Domain.Interfaces;

public interface IProductRepository : IDisposable
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product?> GetByBarcodeAsync(string barcode);
    Task<IEnumerable<Product>> GetAllAsync();
    
    void Add(Product product);
    void Update(Product product);
    void Remove(Product product);
}
