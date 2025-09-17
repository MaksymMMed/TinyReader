using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Seed
{
    public class IdentitySeeder
    {
        private readonly UserManager<IdentityAppUser> _userManager;

        public IdentitySeeder(UserManager<IdentityAppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GenerateData()
        {
            var identityUsers = new List<IdentityAppUser>();
            var student = new IdentityAppUser()
            {
                Id = Guid.Parse("student0-0000-0000-0000-000000000000"),
                Email = "student@email.com"
            };

            var teacher1 = new IdentityAppUser()
            {
                Id = Guid.Parse("teacher1-0000-0000-0000-000000000000"),
                Email = "teacher1@email.com"
            };

            var teacher2 = new IdentityAppUser()
            {
                Id = Guid.Parse("teacher2-0000-0000-0000-000000000000"),
                Email = "teacher2@email.com"
            };

            await _userManager.CreateAsync(student, "password123");
            await _userManager.CreateAsync(teacher1, "password123");
            await _userManager.CreateAsync(teacher2, "password123");
        }
    }
}
