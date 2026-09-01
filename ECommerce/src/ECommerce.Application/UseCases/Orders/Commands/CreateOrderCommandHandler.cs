using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Orders.Dtos;
using ECommerce.Application.UseCases.Payments.Dtos;
using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Orders.Commands;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentClient _paymentClient;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IPaymentClient paymentClient)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _paymentClient = paymentClient;
    }

    public async Task<OrderResponse> Handle(
        CreateOrderCommand command,
        CancellationToken ct)
    {
        if (command.Request.Items is null || command.Request.Items.Count == 0)
        {
            throw new InvalidOperationException("Order must contain at least one item");
        }

        var order = new Order(command.UserId);

        foreach (var item in command.Request.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new InvalidOperationException("Item quantity must be greater than zero");
            }

            var product = await _productRepository.GetByIdAsync(item.ProductId, ct);

            if (product is null)
            {
                throw new InvalidOperationException("Product not found");
            }

            order.AddItem(product, item.Quantity);
        }

        var message = "Payment approved.";

        try
        {
            var paymentResponse = await _paymentClient.ProcessPaymentAsync(
                new PaymentRequestDto(order.Id, order.Total));

            if (paymentResponse.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            {
                order.MarkAsPaid();
                message = $"Payment approved. TransactionId: {paymentResponse.TransactionId}";
            }
            else
            {
                order.MarkPaymentRejected();
                message = $"Payment rejected. TransactionId: {paymentResponse.TransactionId}";
            }
        }
        catch (HttpRequestException ex)
        {
            order.MarkPaymentProcessingFailed();
            message = $"The order was created, but the payment could not be processed because PaymentService is unavailable: {ex.Message}";
        }
        catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
        {
            order.MarkPaymentProcessingFailed();
            message = $"The order was created, but the payment request timed out: {ex.Message}";
        }

        await _orderRepository.AddAsync(order, ct);

        return OrderMapper.ToResponse(order, message);
    }
}