using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _ctx;

    public UserRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _ctx.Users.FirstOrDefaultAsync(
            x => x.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _ctx.Users.AddAsync(user);

        await _ctx.SaveChangesAsync();
    }
}
