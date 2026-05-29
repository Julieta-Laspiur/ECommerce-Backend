using ECommerce.Application.Interfaces;
using ECommerce.Application.UseCases.Users.Dtos;

namespace ECommerce.Application.UseCases.Users.Commands;

public class LoginCommandHandler : ILoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponse?> ExecuteAsync(LoginCommand command)
    {
        var request = command.Request;
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
        {
            return null;
        }

        var validPassword = BCrypt.Net.BCrypt.Verify(
            request.Password,
            user.PasswordHash);

        if (!validPassword)
        {
            return null;
        }

        var token = _tokenService.GenerateToken(
            user.Id,
            user.Email,
            user.Role);

        return new AuthResponse(
            token,
            user.Role,
            user.Email);
    }
}
