using Supermercado.Domain.Enums;

namespace Supermercado.Application.DTOs.Sale;

public record CreateSaleDto(Guid? CustomerId, IEnumerable<CreateSaleItemDto> Items, IEnumerable<CreatePaymentDto> Payments);
public record CreateSaleItemDto(Guid ProductId, int Quantity);
public record CreatePaymentDto(PaymentMethod Method, decimal Amount);
public record CreateSaleResponseDto(Guid SaleId, string? PixQrCode, string? PixCopiaECola);

public record SaleOutputDto(Guid Id, DateTime SaleDate, decimal TotalAmount, Guid? CashierId, Guid? CustomerId, IEnumerable<SaleItemOutputDto> Items, IEnumerable<PaymentOutputDto> Payments);
public record SaleItemOutputDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal TotalPrice);
public record PaymentOutputDto(PaymentMethod Method, decimal Amount, DateTime PaymentDate);
