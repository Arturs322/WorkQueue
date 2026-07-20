using Microsoft.EntityFrameworkCore;
using WorkQueue.Application.Interfaces.Users;
using WorkQueue.DataAccess;
using WorkQueue.Domain.Entities;

namespace WorkQueue.Infrastructure.Repositories.Users
{
    public class UserRepository(ApplicationDbContext _context) : IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
