using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Supermercado.Application.DTOs.Product;
using Supermercado.Application.Interfaces;
using Supermercado.Application.Common;

namespace Supermercado.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductAppService _productAppService;

    public ProductsController(IProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productAppService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productAppService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Funcionario},{Roles.Chefe}")]
    public async Task<IActionResult> Register([FromBody] RegisterProductInputDto input)
    {
        try
        {
            var productId = await _productAppService.RegisterProductAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = productId }, new { id = productId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}/price")]
    [Authorize(Roles = $"{Roles.Funcionario},{Roles.Chefe}")]
    public async Task<IActionResult> UpdatePrice(Guid id, [FromBody] UpdateProductPriceInputDto input)
    {
        try
        {
            await _productAppService.UpdateProductPriceAsync(id, input);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Chefe)]
    public async Task<IActionResult> Remove(Guid id)
    {
        try
        {
            await _productAppService.RemoveProductAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
