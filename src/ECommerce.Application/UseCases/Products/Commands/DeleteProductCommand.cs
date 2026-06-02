using MediatR;

namespace ECommerce.Application.UseCases.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest;
