using ECommerce.Application.UseCases.Orders.Dtos;

namespace ECommerce.Application.UseCases.Orders.Commands;

public record CreateOrderCommand(
    Guid UserId,
    CreateOrderRequest Request);
