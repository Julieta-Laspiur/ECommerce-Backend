namespace ECommerce.Domain.ValueObjects;

public sealed record Quantity
{
    private Quantity(int value)
    {
        Value = value;
    }

    public int Value { get; }

    public static Quantity Create(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero");
        }

        return new Quantity(value);
    }
}
