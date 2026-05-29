using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Repositories;

namespace ECommerce.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
