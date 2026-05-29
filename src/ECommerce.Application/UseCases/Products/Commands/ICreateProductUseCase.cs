using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Commands;

public interface ICreateProductUseCase
{
    Task<Product> ExecuteAsync(
        string name,
        string description,
        decimal price,
        int stock,
        Guid categoryId);
}
