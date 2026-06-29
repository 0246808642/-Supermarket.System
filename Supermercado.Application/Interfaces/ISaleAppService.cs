using Supermercado.Application.DTOs.Sale;

namespace Supermercado.Application.Interfaces;

public interface ISaleAppService
{
    Task<Guid> CreateSaleAsync(CreateSaleDto input);
    Task<SaleOutputDto?> GetSaleByIdAsync(Guid id);
    Task<IEnumerable<SaleOutputDto>> GetAllSalesAsync();
}
