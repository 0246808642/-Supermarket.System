using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Application.DTOs.Address;
using Supermercado.Application.Interfaces;

namespace Supermercado.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressesController : ControllerBase
{
    private readonly IAddressAppService _addressAppService;

    public AddressesController(IAddressAppService addressAppService)
    {
        _addressAppService = addressAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyAddresses()
    {
        var result = await _addressAppService.GetMyAddressesAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAddress([FromBody] CreateAddressDto input)
    {
        try
        {
            var id = await _addressAppService.AddAddressAsync(input);
            return Created(string.Empty, new { id });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveAddress(Guid id)
    {
        try
        {
            await _addressAppService.RemoveAddressAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
