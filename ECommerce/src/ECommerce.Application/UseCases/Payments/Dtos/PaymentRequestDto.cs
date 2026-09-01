namespace ECommerce.Application.UseCases.Payments.Dtos;

public record PaymentRequestDto(
    Guid OrderId,
    decimal Amount);