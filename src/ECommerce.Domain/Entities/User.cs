using ECommerce.Domain.ValueObjects;

namespace ECommerce.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public DateTime CreatedAt { get; set; }

    public User()
    {
    }

    public User(
        string email,
        string name,
        string passwordHash)
    {
        var userEmail = EmailAddress.Create(email);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("User name is required");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required");
        }

        Id = Guid.NewGuid();
        Email = userEmail.Value;
        Name = name.Trim();
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }
}
