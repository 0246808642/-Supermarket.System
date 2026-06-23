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

    public ProductAppService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductOutputDto> RegisterProductAsync(RegisterProductInputDto input)
    {
        var currentDate = DateTime.UtcNow;

        // Orchestration: Instantiating Value Objects using rigid primitives
        var barcode = new Barcode(input.Barcode);
        var price = new Money(input.Price);

        // Business rules and invariants are protected by the Product entity
        var product = new Product(input.Name, input.Description, barcode, price, input.CategoryId, input.ExpirationDate, input.ExpirationDiscountPercentage, currentDate);

        _productRepository.Add(product);
        
        var success = await _unitOfWork.CommitAsync();
        
        if (!success)
            throw new Exception("Houve um erro ao salvar o produto.");

        return MapToOutput(product, currentDate);
    }

    public async Task UpdateProductPriceAsync(Guid id, UpdateProductPriceInputDto input)
    {
        var product = await _productRepository.GetByIdAsync(id);
        
        if (product is null)
            throw new Exception("Produto não encontrado.");

        var newPrice = new Money(input.NewPrice);

        // The entity handles the domain logic for price updates
        product.UpdatePrice(newPrice);

        _productRepository.Update(product);
        
        var success = await _unitOfWork.CommitAsync();
        
        if (!success)
            throw new Exception("Houve um erro ao atualizar o preço do produto.");
    }

    public async Task<ProductOutputDto?> GetProductByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return null;

        var currentDate = DateTime.UtcNow;
        return MapToOutput(product, currentDate);
    }

    public async Task<IEnumerable<ProductOutputDto>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();

        var currentDate = DateTime.UtcNow;
        return products.Select(p => MapToOutput(p, currentDate));
    }

    // Simple manual mapper for demonstration. In a real scenario, AutoMapper could be used.
    private static ProductOutputDto MapToOutput(Product product, DateTime currentDate)
    {
        return new ProductOutputDto(
            product.Id,
            product.Name,
            product.Description,
            product.Barcode.Code,
            product.Price.Value,
            product.StockQuantity,
            product.IsActive,
            product.CategoryId,
            product.ExpirationDate,
            product.ExpirationDiscountPercentage,
            product.IsAvailableForSale(currentDate),
            product.GetCurrentPrice(currentDate).Value
        );
    }
}
