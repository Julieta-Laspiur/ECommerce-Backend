using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Queries;

public interface IGetAllProductsUseCase
{
    Task<IEnumerable<Product>> ExecuteAsync();
}
