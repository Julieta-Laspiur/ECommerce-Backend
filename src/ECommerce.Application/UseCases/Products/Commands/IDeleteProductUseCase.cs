namespace ECommerce.Application.UseCases.Products.Commands;

public interface IDeleteProductUseCase
{
    Task ExecuteAsync(Guid id, CancellationToken ct = default);
}
