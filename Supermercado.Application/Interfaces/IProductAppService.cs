using Supermercado.Application.DTOs.Product;

namespace Supermercado.Application.Interfaces;

public interface IProductAppService
{
    Task<Guid> RegisterProductAsync(RegisterProductInputDto input);
    Task UpdateProductPriceAsync(Guid id, UpdateProductPriceInputDto input);
    Task<object?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<object>> GetAllProductsAsync();
    Task RemoveProductAsync(Guid id);
}
