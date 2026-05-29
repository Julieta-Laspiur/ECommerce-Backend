using ECommerce.Application.UseCases.Orders.Dtos;

namespace ECommerce.Application.UseCases.Orders.Commands;

public interface ICreateOrderUseCase
{
    Task<OrderResponse> ExecuteAsync(
        CreateOrderCommand command,
        CancellationToken ct = default);
}
