using ECommerce.Application.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.Application.UseCases.Orders.Commands;

public record CreateOrderCommand(
    Guid UserId,
    CreateOrderRequest Request) : IRequest<OrderResponse>;
