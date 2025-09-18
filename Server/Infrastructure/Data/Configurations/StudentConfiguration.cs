using Domain.Entities;
using Infrastructure.Data.Seed.BasicEntities;
using Infrastructure.Data.Seed.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class StudentConfiguration : AppUserConfiguration<Student>
    {
        public override void Configure(EntityTypeBuilder<Student> builder)
        {
            base.Configure(builder);

            // Sedding many-to-many classroom-student relationship data only in classroom configuration

            builder.HasMany(x => x.Ratings)
                   .WithOne(x => x.Student)
                   .HasForeignKey(x => x.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Student_Ratings");

            builder.HasData(StudentSeeder.Students);
        }
    }
}
