using Domain.Entities;

namespace Infrastructure.Data.Seed.BasicEntities
{
    public static class ClassroomSeeder
    {
        public static List<Classroom> Classrooms = [

            new Classroom
            {
                Id = Guid.Parse("d291f1ee-6c54-4b01-90e6-d701748f0851"),
                Name = "Classroom 1",
                TeacherId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"),
                IsActive = true,
                CreatedAt = new DateTime(2023, 1, 1)
            },
            new Classroom
            {
                Id = Guid.Parse("d292f1ee-6c54-4b01-90e6-d701748f0851"),
                Name = "Classroom 2",
                TeacherId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"),
                IsActive = true,
                CreatedAt = new DateTime(2023, 1, 1).AddDays(-180)
            },
            new Classroom
            {
                Id = Guid.Parse("d293f1ee-6c54-4b01-90e6-d701748f0851"),
                Name = "Classroom 3",
                TeacherId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"),
                IsActive = false,
                CreatedAt = new DateTime(2023, 1, 1).AddDays(-365)
            },
            new Classroom
            {
                Id = Guid.Parse("d294f1ee-6c54-4b01-90e6-d701748f0851"),
                Name = "Classroom 4",
                TeacherId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0852"),
                IsActive = true,
                CreatedAt = new DateTime(2023, 1, 1)
            },
            new Classroom
            {
                Id = Guid.Parse("d295f1ee-6c54-4b01-90e6-d701748f0851"),
                Name = "Classroom 5",
                TeacherId = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0852"),
                IsActive = true,
                CreatedAt = new DateTime(2023, 1, 1)
            }

        ];
    }
}
