using Microsoft.AspNetCore.Identity;
using WorkQueue.Domain.Entities;
using WorkQueue.Domain.Enums;

namespace WorkQueue.DataAccess
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext db)
        {
            if (db.Users.Any())
                return;


            var hasher = new PasswordHasher<User>();

            var orgA = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Organization A"
            };

            var orgB = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Organization B"
            };

            db.Organizations.AddRange(orgA, orgB);

            var managerA = new User
            {
                Id = Guid.NewGuid(),
                Email = "managerA@test.com",
                Organization = orgA,
                Role = UserRole.Manager
            };

            managerA.PasswordHash = hasher.HashPassword(
                managerA,
                "Password123!"
            );

            var memberA = new User
            {
                Id = Guid.NewGuid(),
                Email = "memberA@test.com",
                Organization = orgA,
                Role = UserRole.Member
            };

            memberA.PasswordHash = hasher.HashPassword(
                memberA,
                "Password123!"
            );

            var managerB = new User
            {
                Id = Guid.NewGuid(),
                Email = "managerB@test.com",
                Organization = orgB,
                Role = UserRole.Manager
            };

            managerB.PasswordHash = hasher.HashPassword(
                managerB,
                "Password123!"
            );

            var memberB = new User
            {
                Id = Guid.NewGuid(),
                Email = "memberB@test.com",
                Organization = orgB,
                Role = UserRole.Member
            };

            memberB.PasswordHash = hasher.HashPassword(
                memberB,
                "Password123!"
            );

            db.Users.AddRange(
                managerA,
                memberA,
                managerB,
                memberB
            );

            db.SaveChanges();
        }
    }
}