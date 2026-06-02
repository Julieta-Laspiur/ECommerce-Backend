using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem()
    {
    }

    public OrderItem(
        Guid orderId,
        Guid productId,
        decimal unitPrice,
        int quantity)
    {
        if (orderId == Guid.Empty)
        {
            throw new ArgumentException("Order is required");
        }

        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product is required");
        }

        var price = Money.Create(unitPrice);
        var requestedQuantity =
            ECommerce.Domain.ValueObjects.Quantity.Create(quantity);

        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        UnitPrice = price.Value;
        Quantity = requestedQuantity.Value;
    }
}
