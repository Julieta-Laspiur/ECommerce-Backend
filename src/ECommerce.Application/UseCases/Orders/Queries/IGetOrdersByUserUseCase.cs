using ECommerce.Application.UseCases.Orders.Dtos;

namespace ECommerce.Application.UseCases.Orders.Queries;

public interface IGetOrdersByUserUseCase
{
    Task<IEnumerable<OrderResponse>> ExecuteAsync(
        Guid userId,
        CancellationToken ct = default);
}
