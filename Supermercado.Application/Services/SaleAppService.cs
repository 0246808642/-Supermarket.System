using Supermercado.Application.DTOs.Sale;
using Supermercado.Application.Interfaces;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;

namespace Supermercado.Application.Services;

public class SaleAppService : ISaleAppService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public SaleAppService(ISaleRepository saleRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Guid> CreateSaleAsync(CreateSaleDto input)
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var sale = new Sale(_currentUser.UserId.Value, DateTime.UtcNow);

        foreach (var itemDto in input.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
            if (product is null)
                throw new Exception($"Produto {itemDto.ProductId} não encontrado.");

            sale.AddItem(product, itemDto.Quantity, DateTime.UtcNow);
            
            product.RemoveStock(itemDto.Quantity, DateTime.UtcNow);
            _productRepository.Update(product);
        }

        _saleRepository.Add(sale);
        
        var success = await _unitOfWork.CommitAsync();
        if (!success)
            throw new Exception("Erro ao finalizar a venda.");

        return sale.Id;
    }

    public async Task<SaleOutputDto?> GetSaleByIdAsync(Guid id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale is null) return null;
        
        return MapToDto(sale);
    }

    public async Task<IEnumerable<SaleOutputDto>> GetAllSalesAsync()
    {
        var sales = await _saleRepository.GetAllAsync();
        return sales.Select(MapToDto);
    }

    private static SaleOutputDto MapToDto(Sale s)
    {
        return new SaleOutputDto(
            s.Id,
            s.SaleDate,
            s.TotalAmount.Value,
            s.CashierId,
            s.Items.Select(i => new SaleItemOutputDto(
                i.ProductId,
                i.Product?.Name ?? "Desconhecido",
                i.Quantity,
                i.UnitPrice.Value,
                i.TotalPrice.Value
            ))
        );
    }
}
