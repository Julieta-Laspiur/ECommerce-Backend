using ECommerce.Application.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.Application.UseCases.Orders.Queries;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderResponse?>;
