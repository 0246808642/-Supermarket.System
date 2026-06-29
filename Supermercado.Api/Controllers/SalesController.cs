using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Application.Common;
using Supermercado.Application.DTOs.Sale;
using Supermercado.Application.Interfaces;

namespace Supermercado.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleAppService _saleAppService;

    public SalesController(ISaleAppService saleAppService)
    {
        _saleAppService = saleAppService;
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Funcionario},{Roles.Chefe}")]
    public async Task<IActionResult> Create([FromBody] CreateSaleDto input)
    {
        try
        {
            var saleId = await _saleAppService.CreateSaleAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = saleId }, new { id = saleId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{Roles.Funcionario},{Roles.Chefe}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var sale = await _saleAppService.GetSaleByIdAsync(id);
        if (sale == null)
            return NotFound();

        return Ok(sale);
    }

    [HttpGet]
    [Authorize(Roles = $"{Roles.Funcionario},{Roles.Chefe}")]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _saleAppService.GetAllSalesAsync();
        return Ok(sales);
    }
}
