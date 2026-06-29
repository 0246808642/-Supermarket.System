using Supermercado.Application.DTOs.Sale;
using Supermercado.Application.Interfaces;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Enums;
using Supermercado.Domain.Interfaces;

namespace Supermercado.Application.Services;

public class SaleAppService : ISaleAppService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly IStockMovementRepository _stockMovementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;
    private readonly IPaymentGatewayService _paymentGatewayService;

    public SaleAppService(ISaleRepository saleRepository, IProductRepository productRepository, IStockMovementRepository stockMovementRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser, IPaymentGatewayService paymentGatewayService)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _stockMovementRepository = stockMovementRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _paymentGatewayService = paymentGatewayService;
    }

    public async Task<CreateSaleResponseDto> CreateSaleAsync(CreateSaleDto input)
    {
        if (_currentUser.UserId is null)
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var customerId = input.CustomerId ?? _currentUser.UserId.Value;
        var sale = new Sale(null, customerId, DateTime.UtcNow);

        foreach (var itemDto in input.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
            if (product is null)
                throw new Exception($"Produto {itemDto.ProductId} não encontrado.");

            sale.AddItem(product, itemDto.Quantity, DateTime.UtcNow);
            
            product.RemoveStock(itemDto.Quantity, DateTime.UtcNow);
            _productRepository.Update(product);

            var movement = new StockMovement(product.Id, itemDto.Quantity, StockMovementType.Saida, StockMovementReason.Venda, _currentUser.UserId.Value);
            _stockMovementRepository.Add(movement);
        }

        string? pixQrCode = null;
        string? pixCopiaECola = null;

        if (input.Payments != null)
        {
            foreach (var paymentDto in input.Payments)
            {
                sale.AddPayment(paymentDto.Method, paymentDto.Amount);

                if (paymentDto.Method == PaymentMethod.Pix)
                {
                    var pixResult = await _paymentGatewayService.CreatePixPaymentAsync(paymentDto.Amount, $"Supermercado - Venda", "cliente@supermercado.com");
                    pixQrCode = pixResult.QrCodeBase64;
                    pixCopiaECola = pixResult.QrCode;
                }
            }
        }

        _saleRepository.Add(sale);
        
        var success = await _unitOfWork.CommitAsync();
        if (!success)
            throw new Exception("Erro ao finalizar a venda.");

        return new CreateSaleResponseDto(sale.Id, pixQrCode, pixCopiaECola);
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
            s.CustomerId,
            s.Items.Select(i => new SaleItemOutputDto(
                i.ProductId,
                i.Product?.Name ?? "Desconhecido",
                i.Quantity,
                i.UnitPrice.Value,
                i.TotalPrice.Value
            )),
            s.Payments.Select(p => new PaymentOutputDto(
                p.Method,
                p.Amount.Value,
                p.PaymentDate
            ))
        );
    }
}
