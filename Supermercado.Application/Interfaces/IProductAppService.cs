using Supermercado.Application.DTOs.Product;

namespace Supermercado.Application.Interfaces;

public interface IProductAppService
{
    Task<Guid> RegisterProductAsync(RegisterProductInputDto input);
    Task UpdateProductPriceAsync(Guid id, UpdateProductPriceInputDto input);
    Task UpdateProductAsync(Guid id, UpdateProductInputDto input);
    Task ActivateProductAsync(Guid id);
    Task DeactivateProductAsync(Guid id);
    Task UpdateDiscountPercentageAsync(Guid id, UpdateDiscountPercentageInputDto input);
    Task AddStockAsync(Guid id, StockInputDto input);
    Task RemoveStockAsync(Guid id, StockInputDto input);
    Task<object?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<object>> GetAllProductsAsync();
    Task<IEnumerable<object>> GetExpiringProductsAsync(int days);
    Task RemoveProductAsync(Guid id);
}
