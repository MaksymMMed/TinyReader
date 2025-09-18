using Domain.Entities;

namespace Infrastructure.Data.Seed.BasicEntities
{
    public static class StudentSeeder
    {
        public static List<Student> Students { get; } = Generate();

        private static List<Student> Generate()
        {
            var list = new List<Student>();
            for (int i = 1; i <= 20; i++)
            {
                list.Add(new Student
                {
                    Id = Guid.Parse($"00000000-0000-0000-0000-{i.ToString().PadLeft(12, '0')}"),
                    Name = $"User{i}",
                    Surname = $"Surname{i}",
                    Email = $"test{i}@email.com"
                });
            }
            return list;
        }
    }

}