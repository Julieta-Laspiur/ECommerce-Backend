using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<Product?>;
