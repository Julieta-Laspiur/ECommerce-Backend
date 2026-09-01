using ECommerce.Domain.Entities;

namespace ECommerce.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task AddAsync(User user);
}