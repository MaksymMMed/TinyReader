using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Seed
{
    public class IdentitySeeder
    {
        public static List<IdentityAppUser> IdentityUsers { get; set; } = GenerateData();
        private static List<IdentityAppUser> GenerateData()
        {
            PasswordHasher<IdentityAppUser> hasher = new PasswordHasher<IdentityAppUser>();
            var identityUsers = new List<IdentityAppUser>();
            var student = new IdentityAppUser()
            {
                Id = Guid.Parse("student0-0000-0000-0000-000000000000"),
                Email = "student@email.com",
            };
            student.PasswordHash = hasher.HashPassword(student, "password123");

            var teacher1 = new IdentityAppUser()
            {
                Id = Guid.Parse("teacher1-0000-0000-0000-000000000000"),
                Email = "teacher1@email.com"
            };
            teacher1.PasswordHash = hasher.HashPassword(teacher1, "password123");

            var teacher2 = new IdentityAppUser()
            {
                Id = Guid.Parse("teacher2-0000-0000-0000-000000000000"),
                Email = "teacher2@email.com"
            };
            teacher2.PasswordHash = hasher.HashPassword(teacher2, "password");
            
            identityUsers.Add(student);
            identityUsers.Add(teacher1);
            identityUsers.Add(teacher2);
            return identityUsers;
        }
    }
}
