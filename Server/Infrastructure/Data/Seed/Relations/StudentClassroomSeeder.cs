using Infrastructure.Data.Seed.BasicEntities;

namespace Infrastructure.Data.Seed.Relations
{
    public class StudentClassroomSeeder
    {
        public List<object> GenerateData()
        {
            var classrooms = new ClassroomSeeder().Entities;
            var students = new StudentSeeder().Entities;

            var classroomStudents = new List<object>(students.Count);

            for (int i = 0; i < students.Count; i++ )
            {
                var student = students[i];
                var classroom = classrooms[i % classrooms.Count];
                classroomStudents.Add(new
                {
                    ClassroomsId = classroom.Id,
                    StudentsId = student.Id
                });
            }
            return classroomStudents;
        }
    }
}
