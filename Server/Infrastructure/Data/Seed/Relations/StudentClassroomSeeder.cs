using Domain.Entities;
using Infrastructure.Data.Seed.BasicEntities;

namespace Infrastructure.Data.Seed.Relations
{
    public class StudentClassroomSeeder
    {
        private static List<Student> Students { get; } = StudentSeeder.Students;
        private static List<Classroom> Classrooms { get; } = ClassroomSeeder.Classrooms;
        public static List<object> Data = GenerateClassroomStudents(Students, Classrooms);

        private static List<object> GenerateClassroomStudents(List<Student> students,List<Classroom> classrooms)
        {
            var classroomStudents = new List<object>(students.Count);

            for (int i = 0; i < students.Count; i++)
            {
                var student = students[i];
                var classroom = classrooms[i % classrooms.Count];
                classroomStudents.Add(new
                {
                    ClassroomId = classroom.Id,
                    StudentId = student.Id
                });
            }
            return classroomStudents;
        }
    }
}


//var classrooms = new List<string>()
//    {
//        "d295f1ee-6c54-4b01-90e6-d701748f0851",
//        "d294f1ee-6c54-4b01-90e6-d701748f0851",
//        "d293f1ee-6c54-4b01-90e6-d701748f0851",
//        "d292f1ee-6c54-4b01-90e6-d701748f0851",
//        "d291f1ee-6c54-4b01-90e6-d701748f0851",
//    };
