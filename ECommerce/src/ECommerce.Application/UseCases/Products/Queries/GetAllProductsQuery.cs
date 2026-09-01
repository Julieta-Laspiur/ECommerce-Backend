using ECommerce.Domain.Entities;
using MediatR;

namespace ECommerce.Application.UseCases.Products.Queries;

public record GetAllProductsQuery() : IRequest<IEnumerable<Product>>;
