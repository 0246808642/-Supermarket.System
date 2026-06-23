using Supermercado.Application.DTOs.Category;
using Supermercado.Application.Interfaces;
using Supermercado.Domain.Entities;
using Supermercado.Domain.Interfaces;

namespace Supermercado.Application.Services;

public class CategoryAppService : ICategoryAppService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryAppService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryOutputDto> RegisterCategoryAsync(RegisterCategoryInputDto input)
    {
        var category = new Category(input.Name, input.Description);

        await _categoryRepository.AddAsync(category);
        
        var success = await _unitOfWork.CommitAsync();
        
        if (!success)
            throw new Exception("Houve um erro ao salvar a categoria.");

        return MapToOutput(category);
    }

    public async Task UpdateCategoryAsync(Guid id, UpdateCategoryInputDto input)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        
        if (category is null)
            throw new Exception("Categoria não encontrada.");

        category.UpdateDetails(input.Name, input.Description);

        await _categoryRepository.UpdateAsync(category);
        
        var success = await _unitOfWork.CommitAsync();
        
        if (!success)
            throw new Exception("Houve um erro ao atualizar a categoria.");
    }

    public async Task<CategoryOutputDto?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category is null)
            return null;

        return MapToOutput(category);
    }

    public async Task<IEnumerable<CategoryOutputDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(MapToOutput);
    }

    private static CategoryOutputDto MapToOutput(Category category)
    {
        return new CategoryOutputDto(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive
        );
    }
}
