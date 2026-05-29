using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Queries;

public class GetAllProductsQuery : IGetAllProductsUseCase
{
    private readonly IProductRepository _repository;

    public GetAllProductsQuery(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> ExecuteAsync()
    {
        return await _repository.GetAllAsync();
    }
}
