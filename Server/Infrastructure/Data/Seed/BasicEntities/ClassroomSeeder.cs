using Domain.Entities;

namespace Infrastructure.Data.Seed.BasicEntities
{
    public class ClassroomSeeder : Seeder<Classroom>
    {
        public ClassroomSeeder() 
        {
            Entities = new List<Classroom>()
            {
                new Classroom
                {
                    Id = Guid.Parse("classroom1-0000-0000-0000-000000000000"),
                    Name = "Classroom 1",
                    TeacherId = Guid.Parse("teacher1-0000-0000-0000-000000000000"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Classroom
                {
                    Id = Guid.Parse("classroom2-0000-0000-0000-000000000000"),
                    Name = "Classroom 2",
                    TeacherId = Guid.Parse("teacher1-0000-0000-0000-000000000000"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(180)
                },
                new Classroom
                {
                    Id = Guid.Parse("classroom3-0000-0000-0000-000000000000"),
                    Name = "Classroom 3",
                    TeacherId = Guid.Parse("teacher1-0000-0000-0000-000000000000"),
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow - TimeSpan.FromDays(365)
                },
                new Classroom
                {
                    Id = Guid.Parse("classroom4-0000-0000-0000-000000000000"),
                    Name = "Classroom 4",
                    TeacherId = Guid.Parse("teacher2-0000-0000-0000-000000000000"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Classroom
                {
                    Id = Guid.Parse("classroom5-0000-0000-0000-000000000000"),
                    Name = "Classroom 5",
                    TeacherId = Guid.Parse("teacher2-0000-0000-0000-000000000000"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };
        }
    }
}
