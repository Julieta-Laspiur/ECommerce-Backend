using ECommerce.Application.UseCases.Payments.Dtos;

namespace ECommerce.Application.Interfaces;

public interface IPaymentClient
{
    Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request);
}