using MediatR;
using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;
using PaymentService.Domain.Enums;

namespace PaymentService.Application.Commands;

public class ProcessPaymentCommandHandler
    : IRequestHandler<ProcessPaymentCommand, PaymentResponseDto>
{
    private const decimal ApprovalLimit = 50000m;

    public Task<PaymentResponseDto> Handle(
        ProcessPaymentCommand request,
        CancellationToken cancellationToken)
    {
        var status = request.PaymentRequest.Amount <= ApprovalLimit
            ? PaymentStatus.Approved
            : PaymentStatus.Rejected;

        var payment = new Payment(
            request.PaymentRequest.Amount,
            Guid.NewGuid().ToString(),
            status);

        var response = new PaymentResponseDto
        {
            Status = payment.Status.ToString(),
            TransactionId = payment.TransactionId
        };

        return Task.FromResult(response);
    }
}
