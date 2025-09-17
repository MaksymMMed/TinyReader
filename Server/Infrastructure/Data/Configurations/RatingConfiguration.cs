using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired();

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.AppUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Student_Ratings");

            builder.HasOne(x => x.Exercise)
                .WithMany(x => x.Ratings)
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Exercise_Ratings");
        }
    }
}