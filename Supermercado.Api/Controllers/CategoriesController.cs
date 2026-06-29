using Microsoft.AspNetCore.Mvc;
using Supermercado.Application.DTOs.Category;
using Supermercado.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Supermercado.Application.Common;

namespace Supermercado.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryAppService _categoryAppService;

    public CategoriesController(ICategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryAppService.GetAllCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var category = await _categoryAppService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Funcionario},{Roles.Chefe}")]
    public async Task<IActionResult> Register([FromBody] RegisterCategoryInputDto input)
    {
        try
        {
            var category = await _categoryAppService.RegisterCategoryAsync(input);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = $"{Roles.Funcionario},{Roles.Chefe}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryInputDto input)
    {
        try
        {
            await _categoryAppService.UpdateCategoryAsync(id, input);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
