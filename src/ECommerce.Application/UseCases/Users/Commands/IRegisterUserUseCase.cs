namespace ECommerce.Application.UseCases.Users.Commands;

public interface IRegisterUserUseCase
{
    Task ExecuteAsync(RegisterUserCommand command);
}
