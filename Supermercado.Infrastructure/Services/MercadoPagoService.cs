using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Supermercado.Application.DTOs.Payment;
using Supermercado.Application.Interfaces;

namespace Supermercado.Infrastructure.Services;

public class MercadoPagoService : IPaymentGatewayService
{
    private readonly HttpClient _httpClient;
    public MercadoPagoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PixPaymentResultDto> CreatePixPaymentAsync(decimal amount, string description, string email)
    {
        // Fallback simulado caso o token não esteja configurado no appsettings.json
        if (string.IsNullOrWhiteSpace(_httpClient.DefaultRequestHeaders.Authorization?.Parameter))
        {
            return new PixPaymentResultDto(
                DateTime.UtcNow.Ticks, 
                "00020126360014br.gov.bcb.pix0114+5511999999999...", 
                "QkFTRTY0X01PQ0tfUVI=" // Base64 falso para testes
            );
        }

        var idempotencyKey = Guid.NewGuid().ToString();
        _httpClient.DefaultRequestHeaders.Remove("X-Idempotency-Key");
        _httpClient.DefaultRequestHeaders.Add("X-Idempotency-Key", idempotencyKey);

        var payload = new
        {
            transaction_amount = amount,
            description = description,
            payment_method_id = "pix",
            payer = new { email = email }
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("v1/payments", content);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao gerar PIX no Mercado Pago: {errorBody}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseBody);
        var root = document.RootElement;

        var paymentId = root.GetProperty("id").GetInt64();
        var pointOfInteraction = root.GetProperty("point_of_interaction");
        var transactionData = pointOfInteraction.GetProperty("transaction_data");
        
        var qrCode = transactionData.GetProperty("qr_code").GetString() ?? "";
        var qrCodeBase64 = transactionData.GetProperty("qr_code_base64").GetString() ?? "";

        return new PixPaymentResultDto(paymentId, qrCode, qrCodeBase64);
    }
}
