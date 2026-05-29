using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Products.Commands;

public class CreateProductCommand : ICreateProductUseCase
{
    private readonly IProductRepository _repository;

    public CreateProductCommand(
        IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product> ExecuteAsync(
        string name,
        string description,
        decimal price,
        int stock,
        Guid categoryId)
    {
        var product = new Product(
            name,
            description,
            price,
            stock,
            categoryId);

        await _repository.AddAsync(product);

        return product;
    }
}
