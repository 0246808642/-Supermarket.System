using Supermercado.Application.DTOs.Product;

namespace Supermercado.Application.Interfaces;

public interface IProductAppService
{
    Task<ProductOutputDto> RegisterProductAsync(RegisterProductInputDto input);
    Task UpdateProductPriceAsync(Guid id, UpdateProductPriceInputDto input);
    Task<ProductOutputDto?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductOutputDto>> GetAllProductsAsync();
}
