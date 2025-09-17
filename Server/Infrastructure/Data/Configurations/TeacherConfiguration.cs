using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TeacherConfiguration :AppUserConfiguration<Teacher>
    {
        public override void Configure(EntityTypeBuilder<Teacher> builder)
        {
            base.Configure(builder);

            builder.HasMany(x => x.TeacherClassrooms)
                   .WithOne(x => x.Teacher)
                   .HasForeignKey(x => x.TeacherId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .HasConstraintName("FK_Teacher_Classrooms");
        }
    }
}
