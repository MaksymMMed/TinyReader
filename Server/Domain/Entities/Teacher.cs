namespace Domain.Entities
{
    public class Teacher : AppUser
    {
        public ICollection<Classroom> TeacherClassrooms { get; set; }

    }
}
