namespace ECommerce.Application.UseCases.Users.Dtos;

public record RegisterRequest(
    string Email,
    string Name,
    string Password,
    string Role = "User");
