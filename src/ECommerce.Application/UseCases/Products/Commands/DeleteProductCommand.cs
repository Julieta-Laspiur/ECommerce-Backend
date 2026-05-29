using ECommerce.Application.Interfaces;

namespace ECommerce.Application.UseCases.Products.Commands;

public class DeleteProductCommand : IDeleteProductUseCase
{
    private readonly IProductRepository _repository;

    public DeleteProductCommand(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(Guid id, CancellationToken ct = default)
    {
        await _repository.DeleteAsync(id, ct);
    }
}
