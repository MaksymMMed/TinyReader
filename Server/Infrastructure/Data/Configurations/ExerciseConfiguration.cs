using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(x => x.Classroom)
                .WithMany(x => x.Exercises)
                .HasForeignKey(x => x.ClassroomId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Classroom_Exercises");

            builder.HasMany(x => x.Ratings)
                .WithOne(x => x.Exercise)
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Exercise_Ratings");
        }
    }
}
