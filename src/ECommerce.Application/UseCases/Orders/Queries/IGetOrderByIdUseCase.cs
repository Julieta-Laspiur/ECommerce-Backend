using ECommerce.Application.UseCases.Orders.Dtos;

namespace ECommerce.Application.UseCases.Orders.Queries;

public interface IGetOrderByIdUseCase
{
    Task<OrderResponse?> ExecuteAsync(
        Guid id,
        CancellationToken ct = default);
}
