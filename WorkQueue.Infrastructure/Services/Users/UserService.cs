using Microsoft.EntityFrameworkCore;
using WorkQueue.Application.Interfaces.Users;
using WorkQueue.DataAccess;
using WorkQueue.Domain.Entities;

namespace WorkQueue.Infrastructure.Services.Users
{
    public class UserService(ApplicationDbContext _context) : IUserService
    {
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
