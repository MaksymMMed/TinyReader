namespace Domain.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public Classroom Classroom { get; set; }
        public Guid ClassroomId { get; set; }
    }
}