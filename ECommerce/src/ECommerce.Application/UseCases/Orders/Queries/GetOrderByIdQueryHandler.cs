using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Orders.Dtos;
using MediatR;

namespace ECommerce.Application.UseCases.Orders.Queries;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderResponse?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdWithItemsAsync(
            request.Id,
            cancellationToken);

        return order is null
            ? null
            : OrderMapper.ToResponse(order);
    }
}
