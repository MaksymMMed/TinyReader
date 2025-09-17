namespace Domain.Entities
{
    public class Student:AppUser
    {
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Classroom> StudentClassrooms { get; set; }
    }
}
