namespace Supermercado.Application.DTOs.ShoppingCart;

public record CartItemInputDto(Guid ProductId, int Quantity);
public record ShoppingCartOutputDto(Guid Id, IEnumerable<CartItemOutputDto> Items);
public record CartItemOutputDto(Guid ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal TotalPrice);
