using ECommerce.Application.UseCases.Users.Dtos;

namespace ECommerce.Application.UseCases.Users.Commands;

public interface ILoginUseCase
{
    Task<AuthResponse?> ExecuteAsync(LoginCommand command);
}
