namespace Domain.Entities
{
    public class Classroom
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public ICollection<AppUser> Students { get; set; }
        public Guid TeacherId { get; set; }
        public AppUser Teacher { get; set; }
    }
}
