using Domain.Entities;

namespace Infrastructure.Data.Seed.BasicEntities
{
    public class StudentSeeder : Seeder<Student>
    {
        public StudentSeeder()
        {
            for (int i = 1; i <= 20; i++)
            {
                Entities.Add(new Student
                {
                    Id = Guid.Parse($"00000000-0000-0000-0000-{i.ToString().PadLeft(12, '0')}"),
                    Name = $"User{i}",
                    Surname = $"Surname{i}",
                    Email = $"test{i}@email.com"
                });
            }
        }
    } 
}