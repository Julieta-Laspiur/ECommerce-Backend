using MediatR;
using PaymentService.Application.DTOs;

namespace PaymentService.Application.Commands;

public record ProcessPaymentCommand(PaymentRequestDto PaymentRequest)
    : IRequest<PaymentResponseDto>;
