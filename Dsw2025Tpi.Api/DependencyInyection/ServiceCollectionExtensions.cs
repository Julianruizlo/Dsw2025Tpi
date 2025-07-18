using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Application.Interfaces;
using Dsw2025Tpi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Dsw2025Tpi.Api.DependencyInyection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        // Repositorios
        services.AddScoped<IRepository, EfRepository>();

        // Servicios de aplicación
        services.AddScoped<IProductsManagementService, ProductsManagementService>();
        services.AddScoped<IOrdersManagementService, OrdersManagementService>();

        // Db Context
        services.AddDbContext<Dsw2025TpiContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Dsw2025Ej15Entities")));

        return services;
    }
}
