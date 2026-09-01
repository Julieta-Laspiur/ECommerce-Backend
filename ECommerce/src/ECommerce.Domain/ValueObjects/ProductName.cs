namespace ECommerce.Domain.ValueObjects;

public sealed record ProductName
{
    private ProductName(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static ProductName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Product name is required");
        }

        var normalized = value.Trim();

        if (normalized.Length > 120)
        {
            throw new ArgumentException("Product name cannot exceed 120 characters");
        }

        return new ProductName(normalized);
    }
}
