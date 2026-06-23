namespace Supermercado.Application.DTOs.Category;

public record CategoryOutputDto(
    Guid Id,
    string Name,
    string Description,
    bool IsActive
);
