using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.UseCases.Users.Commands;

public class RegisterUserCommandHandler : IRegisterUserUseCase
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ExecuteAsync(RegisterUserCommand command)
    {
        var request = command.Request;
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            throw new InvalidOperationException("User already exists");
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User(
            request.Email,
            request.Name,
            passwordHash)
        {
            Role = string.IsNullOrWhiteSpace(request.Role)
                ? "User"
                : request.Role
        };

        await _userRepository.AddAsync(user);
    }
}
