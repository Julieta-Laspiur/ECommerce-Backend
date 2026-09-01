namespace ECommerce.Domain.ValueObjects;

public sealed record Money
{
    private Money(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }

    public static Money Create(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Amount cannot be negative");
        }

        return new Money(decimal.Round(value, 2));
    }
}
