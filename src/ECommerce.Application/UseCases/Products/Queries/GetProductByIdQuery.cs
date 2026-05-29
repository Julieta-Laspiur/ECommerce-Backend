using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Queries;

public class GetProductByIdQuery : IGetProductByIdUseCase
{
    private readonly IProductRepository _repository;

    public GetProductByIdQuery(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product?> ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        return await _repository.GetByIdAsync(id, ct);
    }
}
