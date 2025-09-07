using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<IdentityAppUser,IdentityRole<Guid>,Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
    }
}
