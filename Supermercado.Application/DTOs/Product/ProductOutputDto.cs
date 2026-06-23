namespace Supermercado.Application.DTOs.Product;

public record ProductOutputDto(Guid Id, string Name, decimal CurrentPrice, bool IsAvailable);
