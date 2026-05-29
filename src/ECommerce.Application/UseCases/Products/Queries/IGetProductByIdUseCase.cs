using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Queries;

public interface IGetProductByIdUseCase
{
    Task<Product?> ExecuteAsync(Guid id, CancellationToken ct = default);
}
