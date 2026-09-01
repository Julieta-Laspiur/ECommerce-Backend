using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.Application.UseCases.Orders.Queries;

public class GetOrdersByUserQueryHandler
    : IRequestHandler<GetOrdersByUserQuery, IEnumerable<OrderResponse>>
{
    private readonly IOrderRepository _repository;

    public GetOrdersByUserQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderResponse>> Handle(
        GetOrdersByUserQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await _repository.GetByUserIdAsync(
            request.UserId,
            cancellationToken);

        return orders.Select(OrderMapper.ToResponse);
    }
}
