namespace ECommerce.Application.UseCases.Users.Dtos;

public record LoginRequest(
    string Email,
    string Password);
