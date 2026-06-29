using Supermercado.Application.DTOs.ShoppingCart;

namespace Supermercado.Application.Interfaces;

public interface IShoppingCartAppService
{
    Task<ShoppingCartOutputDto> GetMyCartAsync();
    Task AddItemAsync(CartItemInputDto input);
    Task RemoveItemAsync(Guid productId);
    Task ClearCartAsync();
}
