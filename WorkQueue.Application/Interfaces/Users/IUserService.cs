using WorkQueue.Domain.Entities;

namespace WorkQueue.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
    }
}
