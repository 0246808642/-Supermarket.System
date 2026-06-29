using Microsoft.Extensions.DependencyInjection;
using Supermercado.Application.Interfaces;
using Supermercado.Application.Services;
using Supermercado.Domain.Interfaces;
using Supermercado.Infrastructure.Data;
using Supermercado.Infrastructure.Repositories;

namespace Supermercado.CrossCutting.IoC;

public static class NativeInjectorBootStrapper
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // Application Layer
        services.AddScoped<IProductAppService, ProductAppService>();
        services.AddScoped<ICategoryAppService, CategoryAppService>();
        services.AddScoped<ISaleAppService, SaleAppService>();

        // Infrastructure Layer
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
