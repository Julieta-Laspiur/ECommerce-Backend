using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        var admin = await context.Users.FirstOrDefaultAsync(
            x => x.Email == "admin@test.com");

        if (admin is null)
        {
            context.Users.Add(new User(
                "admin@test.com",
                "Admin",
                BCrypt.Net.BCrypt.HashPassword("Admin123!"))
            {
                Role = "Admin"
            });
        }
        else
        {
            admin.Name = "Admin";
            admin.Role = "Admin";
            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
        }

        var user = await context.Users.FirstOrDefaultAsync(
            x => x.Email == "user@test.com");

        if (user is null)
        {
            context.Users.Add(new User(
                "user@test.com",
                "User",
                BCrypt.Net.BCrypt.HashPassword("User123!"))
            {
                Role = "User"
            });
        }
        else
        {
            user.Name = "User";
            user.Role = "User";
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!");
        }

        await context.SaveChangesAsync();
    }
}
