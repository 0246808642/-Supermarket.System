using Supermercado.Application.DTOs.ShoppingCart;
using Supermercado.Application.Interfaces;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;

namespace Supermercado.Application.Services;

public class ShoppingCartAppService : IShoppingCartAppService
{
    private readonly IShoppingCartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ShoppingCartAppService(IShoppingCartRepository cartRepository, IProductRepository productRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<ShoppingCartOutputDto> GetMyCartAsync()
    {
        if (_currentUser.UserId == null) throw new UnauthorizedAccessException();
        
        var cart = await _cartRepository.GetByCustomerIdAsync(_currentUser.UserId.Value);
        if (cart == null)
            return new ShoppingCartOutputDto(Guid.Empty, Enumerable.Empty<CartItemOutputDto>());

        return MapToDto(cart);
    }

    public async Task AddItemAsync(CartItemInputDto input)
    {
        if (_currentUser.UserId == null) throw new UnauthorizedAccessException();
        
        var product = await _productRepository.GetByIdAsync(input.ProductId);
        if (product == null) throw new Exception("Produto não encontrado.");
        
        var cart = await _cartRepository.GetByCustomerIdAsync(_currentUser.UserId.Value);
        if (cart == null)
        {
            cart = new ShoppingCart(_currentUser.UserId.Value);
            _cartRepository.Add(cart);
        }

        cart.AddItem(input.ProductId, input.Quantity);
        if (cart.Id != Guid.Empty) _cartRepository.Update(cart);

        await _unitOfWork.CommitAsync();
    }

    public async Task RemoveItemAsync(Guid productId)
    {
        if (_currentUser.UserId == null) throw new UnauthorizedAccessException();
        
        var cart = await _cartRepository.GetByCustomerIdAsync(_currentUser.UserId.Value);
        if (cart == null) return;
        
        cart.RemoveItem(productId);
        _cartRepository.Update(cart);
        await _unitOfWork.CommitAsync();
    }

    public async Task ClearCartAsync()
    {
        if (_currentUser.UserId == null) throw new UnauthorizedAccessException();
        
        var cart = await _cartRepository.GetByCustomerIdAsync(_currentUser.UserId.Value);
        if (cart == null) return;
        
        cart.Clear();
        _cartRepository.Update(cart);
        await _unitOfWork.CommitAsync();
    }

    private ShoppingCartOutputDto MapToDto(ShoppingCart cart)
    {
        return new ShoppingCartOutputDto(cart.Id, cart.Items.Select(i => new CartItemOutputDto(
            i.ProductId,
            i.Product?.Name ?? "Desconhecido",
            i.Quantity,
            i.Product?.GetCurrentPrice(DateTime.UtcNow).Value ?? 0,
            (i.Product?.GetCurrentPrice(DateTime.UtcNow).Value ?? 0) * i.Quantity
        )));
    }
}
