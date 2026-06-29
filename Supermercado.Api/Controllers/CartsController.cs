using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Application.DTOs.ShoppingCart;
using Supermercado.Application.Interfaces;

namespace Supermercado.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartsController : ControllerBase
{
    private readonly IShoppingCartAppService _cartAppService;

    public CartsController(IShoppingCartAppService cartAppService)
    {
        _cartAppService = cartAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyCart()
    {
        var result = await _cartAppService.GetMyCartAsync();
        return Ok(result);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] CartItemInputDto input)
    {
        try
        {
            await _cartAppService.AddItemAsync(input);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid productId)
    {
        try
        {
            await _cartAppService.RemoveItemAsync(productId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        try
        {
            await _cartAppService.ClearCartAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
