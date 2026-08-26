namespace ECommerce.Application.UseCases.Payments.Dtos;

public record PaymentResponseDto(
    string Status,
    string TransactionId);