using ECommerce.Application.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.Application.UseCases.Orders.Queries;

public record GetOrdersByUserQuery(Guid UserId)
    : IRequest<IEnumerable<OrderResponse>>;
