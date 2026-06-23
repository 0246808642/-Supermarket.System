using Supermercado.Application.Common;
using Supermercado.Application.DTOs.Product;
using Supermercado.Application.Interfaces;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;
using Supermercado.Domain.ValueObjects;

namespace Supermercado.Application.Services;

public class ProductAppService : IProductAppService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ProductAppService(IProductRepository productRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _productRepository = productRepository;
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
