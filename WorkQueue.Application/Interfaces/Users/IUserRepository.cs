using WorkQueue.Domain.Entities;

namespace WorkQueue.Application.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
