using ECommerce.Application.UseCases.Users.Dtos;
using MediatR;

namespace ECommerce.Application.UseCases.Users.Commands;

public record LoginCommand(LoginRequest Request) : IRequest<AuthResponse?>;
