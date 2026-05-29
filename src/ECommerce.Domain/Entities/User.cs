namespace ECommerce.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "User";

    public DateTime CreatedAt { get; set; }

    // Constructor vacío
    public User()
    {
    }

    public User(
        string email,
        string name,
        string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        Name = name;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }
}