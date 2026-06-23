using Supermercado.Application.DTOs.Category;

namespace Supermercado.Application.Interfaces;

public interface ICategoryAppService
{
    Task<CategoryOutputDto> RegisterCategoryAsync(RegisterCategoryInputDto input);
    Task UpdateCategoryAsync(Guid id, UpdateCategoryInputDto input);
    Task<CategoryOutputDto?> GetCategoryByIdAsync(Guid id);
    Task<IEnumerable<CategoryOutputDto>> GetAllCategoriesAsync();
}
