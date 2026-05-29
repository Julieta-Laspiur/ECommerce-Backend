namespace ECommerce.Application.UseCases.Orders.Dtos;

public record CreateOrderRequest(
    IReadOnlyCollection<CreateOrderItemRequest> Items);

public record CreateOrderItemRequest(
    Guid ProductId,
    int Quantity);
