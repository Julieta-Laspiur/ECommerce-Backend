using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Orders.Dtos;

namespace ECommerce.Application.UseCases.Orders.Queries;

public class GetOrderByIdQuery : IGetOrderByIdUseCase
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdQuery(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderResponse?> ExecuteAsync(
        Guid id,
        CancellationToken ct = default)
    {
        var order = await _repository.GetByIdWithItemsAsync(id, ct);

        return order is null
            ? null
            : OrderMapper.ToResponse(order);
    }
}
