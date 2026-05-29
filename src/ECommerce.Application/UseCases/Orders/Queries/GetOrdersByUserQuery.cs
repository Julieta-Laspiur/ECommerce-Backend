using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Orders.Dtos;

namespace ECommerce.Application.UseCases.Orders.Queries;

public class GetOrdersByUserQuery : IGetOrdersByUserUseCase
{
    private readonly IOrderRepository _repository;

    public GetOrdersByUserQuery(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderResponse>> ExecuteAsync(
        Guid userId,
        CancellationToken ct = default)
    {
        var orders = await _repository.GetByUserIdAsync(userId, ct);

        return orders.Select(OrderMapper.ToResponse);
    }
}
