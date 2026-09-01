namespace ECommerce.Application.UseCases.Orders.Dtos;

public record OrderResponse(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    string Status,
    decimal Total,
    IReadOnlyCollection<OrderItemResponse> Items,
    string Message = "");

public record OrderItemResponse(
    Guid Id,
    Guid ProductId,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal);