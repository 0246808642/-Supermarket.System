namespace Supermercado.Application.DTOs.Payment;

public record PixPaymentResultDto(long ExternalPaymentId, string QrCode, string QrCodeBase64);
