using Domain.Entities;
using Infrastructure.Data.Seed.BasicEntities;
using Infrastructure.Data.Seed.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
    {
        public void Configure(EntityTypeBuilder<Classroom> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Sedding many-to-many classroom-student relationship data only in classroom configuration
            builder.HasMany(x => x.Students)
                .WithMany(x => x.StudentClassrooms)
                .UsingEntity<Dictionary<string, object>>(
                    "ClassroomStudents",
                    j => j.HasOne<Student>()
                          .WithMany()
                          .HasForeignKey("StudentId")
                          .HasConstraintName("FK_ClassroomStudents_Students")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Classroom>()
                          .WithMany()
                          .HasForeignKey("ClassroomId")
                          .HasConstraintName("FK_ClassroomStudents_Classrooms")
                          .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.ToTable("ClassroomStudents");
                        j.HasKey("ClassroomId", "StudentId");
                        j.HasData(StudentClassroomSeeder.Data);
                    });


            builder.HasMany(x => x.Exercises)
                .WithOne(x => x.Classroom)
                .HasForeignKey(x => x.ClassroomId)
                .HasConstraintName("FK_Classroom_Exercises")
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.Teacher)
                .WithMany(x=>x.TeacherClassrooms)
                .HasForeignKey(x => x.TeacherId)
                .HasConstraintName("FK_Teacher_Classroom")
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasData(ClassroomSeeder.Classrooms);
        }
    }
}
