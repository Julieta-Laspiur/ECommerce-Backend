using PaymentService.Domain.Enums;

namespace PaymentService.Domain.Entities;

public class Payment
{
    public Payment(decimal amount, string transactionId, PaymentStatus status)
    {
        Amount = amount;
        TransactionId = transactionId;
        Status = status;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public decimal Amount { get; private set; }

    public string TransactionId { get; private set; }

    public PaymentStatus Status { get; private set; }

    public DateTime CreatedAt { get; private set; }
}
