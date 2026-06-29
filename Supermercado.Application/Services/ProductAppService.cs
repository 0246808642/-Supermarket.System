using Supermercado.Application.Common;
using Supermercado.Application.DTOs.Product;
using Supermercado.Application.Interfaces;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Enums;
using Supermercado.Domain.Interfaces;
using Supermercado.Domain.ValueObjects;

namespace Supermercado.Application.Services;

public class ProductAppService : IProductAppService
{
    private readonly IProductRepository _productRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ProductAppService(IProductRepository productRepository, IStockMovementRepository stockMovementRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _productRepository = productRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Guid> RegisterProductAsync(RegisterProductInputDto input)
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var currentDate = DateTime.UtcNow;
        var barcode = new Barcode(input.Barcode);
        var price = new Money(input.Price);

        var product = new Product(input.Name, input.Description, barcode, price, input.CategoryId, input.ExpirationDate, input.ExpirationDiscountPercentage, currentDate, _currentUser.UserId.Value, currentDate);

        _productRepository.Add(product);
        
        var success = await _unitOfWork.CommitAsync();
        
        if (!success)
            throw new Exception("Houve um erro ao salvar o produto.");

        return product.Id;
    }

    public async Task UpdateProductPriceAsync(Guid id, UpdateProductPriceInputDto input)
    {
        var product = await _productRepository.GetByIdAsync(id);
        
        if (product is null)
            throw new Exception("Produto não encontrado.");

        var newPrice = new Money(input.NewPrice);
        product.UpdatePrice(newPrice);

        _productRepository.Update(product);
        
        var success = await _unitOfWork.CommitAsync();
        
        if (!success)
            throw new Exception("Houve um erro ao atualizar o preço do produto.");
    }

    public async Task UpdateProductAsync(Guid id, UpdateProductInputDto input)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) throw new Exception("Produto não encontrado.");

        product.UpdateDetails(input.Name, input.Description, input.CategoryId);
        _productRepository.Update(product);
        if (!await _unitOfWork.CommitAsync()) throw new Exception("Erro ao atualizar o produto.");
    }

    public async Task ActivateProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) throw new Exception("Produto não encontrado.");

        product.Activate();
        _productRepository.Update(product);
        if (!await _unitOfWork.CommitAsync()) throw new Exception("Erro ao ativar o produto.");
    }

    public async Task DeactivateProductAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) throw new Exception("Produto não encontrado.");

        product.Deactivate();
        _productRepository.Update(product);
        if (!await _unitOfWork.CommitAsync()) throw new Exception("Erro ao desativar o produto.");
    }

    public async Task UpdateDiscountPercentageAsync(Guid id, UpdateDiscountPercentageInputDto input)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) throw new Exception("Produto não encontrado.");

        product.UpdateDiscountPercentage(input.Percentage);
        _productRepository.Update(product);
        if (!await _unitOfWork.CommitAsync()) throw new Exception("Erro ao atualizar o desconto do produto.");
    }

    public async Task AddStockAsync(Guid id, StockInputDto input)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) throw new Exception("Produto não encontrado.");

        product.AddStock(input.Quantity);
        _productRepository.Update(product);

        var movement = new StockMovement(product.Id, input.Quantity, StockMovementType.Entrada, StockMovementReason.AjusteManual, _currentUser.UserId ?? Guid.Empty);
        _stockMovementRepository.Add(movement);

        if (!await _unitOfWork.CommitAsync()) throw new Exception("Erro ao adicionar estoque.");
    }

    public async Task RemoveStockAsync(Guid id, StockInputDto input)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null) throw new Exception("Produto não encontrado.");

        product.RemoveStock(input.Quantity, DateTime.UtcNow);
        _productRepository.Update(product);

        var movement = new StockMovement(product.Id, input.Quantity, StockMovementType.Saida, StockMovementReason.AjusteManual, _currentUser.UserId ?? Guid.Empty);
        _stockMovementRepository.Add(movement);

        if (!await _unitOfWork.CommitAsync()) throw new Exception("Erro ao remover estoque.");
    }

    public async Task<object?> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return null;

        var currentDate = DateTime.UtcNow;
        return _currentUser.Role switch
        {
            Roles.Chefe => MapToAuditDto(product, currentDate),
            Roles.Funcionario => MapToDetailedDto(product, currentDate),
            _ => MapToBaseDto(product, currentDate)
        };
    }

    public async Task<IEnumerable<object>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        var currentDate = DateTime.UtcNow;

        return _currentUser.Role switch
        {
            Roles.Chefe => products.Select(p => MapToAuditDto(p, currentDate)),
            Roles.Funcionario => products.Select(p => MapToDetailedDto(p, currentDate)),
            _ => products.Select(p => MapToBaseDto(p, currentDate))
        };
    }

    public async Task<IEnumerable<object>> GetExpiringProductsAsync(int days)
    {
        var products = await _productRepository.GetExpiringProductsAsync(days);
        var currentDate = DateTime.UtcNow;

        return _currentUser.Role switch
        {
            Roles.Chefe => products.Select(p => MapToAuditDto(p, currentDate)),
            Roles.Funcionario => products.Select(p => MapToDetailedDto(p, currentDate)),
            _ => products.Select(p => MapToBaseDto(p, currentDate))
        };
    }

    public async Task RemoveProductAsync(Guid id)
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var product = await _productRepository.GetByIdAsync(id);
        if (product is null)
            throw new Exception("Produto não encontrado.");

        product.Remove(_currentUser.UserId.Value, DateTime.UtcNow);
        
        _productRepository.Update(product);
        var success = await _unitOfWork.CommitAsync();
        
        if (!success)
            throw new Exception("Houve um erro ao remover o produto.");
    }

    private static ProductOutputDto MapToBaseDto(Product p, DateTime now) =>
        new(p.Id, p.Name, p.GetCurrentPrice(now).Value, p.IsAvailableForSale(now));

    private static ProductOutputDetailedDto MapToDetailedDto(Product p, DateTime now) =>
        new(p.Id, p.Name, p.GetCurrentPrice(now).Value, p.IsAvailableForSale(now),
            p.Description, p.Barcode.Code, p.Price.Value, p.StockQuantity,
            p.IsActive, p.CategoryId, p.ExpirationDate, p.ExpirationDiscountPercentage);

    private static ProductOutputAuditDto MapToAuditDto(Product p, DateTime now) =>
        new(p.Id, p.Name, p.GetCurrentPrice(now).Value, p.IsAvailableForSale(now),
            p.Description, p.Barcode.Code, p.Price.Value, p.StockQuantity,
            p.IsActive, p.CategoryId, p.ExpirationDate, p.ExpirationDiscountPercentage,
            p.CreatedByUserId, p.CreatedAt, p.RemovedByUserId, p.RemovedAt, p.IsRemoved);
}
