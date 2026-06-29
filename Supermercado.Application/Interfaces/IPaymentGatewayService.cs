using Supermercado.Application.DTOs.Payment;

namespace Supermercado.Application.Interfaces;

public interface IPaymentGatewayService
{
    Task<PixPaymentResultDto> CreatePixPaymentAsync(decimal amount, string description, string email);
}
