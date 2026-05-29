using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Orders.Dtos;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Orders.Commands;

public class CreateOrderCommandHandler : ICreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderResponse> ExecuteAsync(
        CreateOrderCommand command,
        CancellationToken ct = default)
    {
        if (command.Request.Items is null || command.Request.Items.Count == 0)
        {
            throw new InvalidOperationException("Order must contain at least one item");
        }

        var order = new Order(command.UserId);

        foreach (var item in command.Request.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new InvalidOperationException("Item quantity must be greater than zero");
            }

            var product = await _productRepository.GetByIdAsync(item.ProductId, ct);

            if (product is null)
            {
                throw new InvalidOperationException("Product not found");
            }

            order.AddItem(product, item.Quantity);
        }

        await _orderRepository.AddAsync(order, ct);

        return OrderMapper.ToResponse(order);
    }
}
