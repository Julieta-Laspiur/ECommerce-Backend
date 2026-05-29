using System.Text.Json.Serialization;

namespace ECommerce.Application.UseCases.Users.Dtos;

public record AuthResponse(
    [property: JsonPropertyName("token")]
    string Token,
    [property: JsonPropertyName("role")]
    string Role,
    [property: JsonPropertyName("email")]
    string Email);
