using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    public Guid CategoryId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Product()
    {
    }

    public Product(
        string name,
        string description,
        decimal price,
        int stock,
        Guid categoryId)
    {
        if (stock < 0)
        {
            throw new ArgumentException("Stock cannot be negative");
        }

        if (categoryId == Guid.Empty)
        {
            throw new ArgumentException("Category is required");
        }

        var productName = ProductName.Create(name);
        var productPrice = Money.Create(price);

        Id = Guid.NewGuid();
        Name = productName.Value;
        Description = description?.Trim() ?? string.Empty;
        Price = productPrice.Value;
        Stock = stock;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdatePrice(decimal newPrice)
    {
        Price = Money.Create(newPrice).Value;
    }

    public void ReduceStock(int quantity)
    {
        var requestedQuantity = Quantity.Create(quantity);

        if (requestedQuantity.Value > Stock)
        {
            throw new InvalidOperationException("Insufficient stock");
        }

        Stock -= requestedQuantity.Value;
    }
}
