using ECommerce.Application.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.Application.UseCases.Users.Commands;

public record RegisterUserCommand(RegisterRequest Request) : IRequest;
