using Domain.Entities;

namespace Infrastructure.Data.Seed.BasicEntities
{
    public class TeacherSeeder
    {
        public static List<Teacher> Teachers = new List<Teacher>()
        {
            new Teacher
            {
                Id = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"),
                Name = "John",
                Surname = "Doe",
                Email = "teacher1@email.com"
            },
            new Teacher
            {
                Id = Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0852"),
                Name = "Jane",
                Surname = "Smith",
                Email = "teacher2@email.com"
            }
        };
    }
}
