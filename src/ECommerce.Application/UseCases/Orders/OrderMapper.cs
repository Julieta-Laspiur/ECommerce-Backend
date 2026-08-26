using ECommerce.Application.UseCases.Orders.Dtos;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Orders;

internal static class OrderMapper
{
    public static OrderResponse ToResponse(Order order)
    {
        return ToResponse(order, string.Empty);
    }

    public static OrderResponse ToResponse(Order order, string message)
    {
        return new OrderResponse(
            order.Id,
            order.UserId,
            order.CreatedAt,
            order.Status.ToString(),
            order.Total,
            order.Items
                .Select(item => new OrderItemResponse(
                    item.Id,
                    item.ProductId,
                    item.UnitPrice,
                    item.Quantity,
                    item.Subtotal))
                .ToList(),
            message);
    }
}