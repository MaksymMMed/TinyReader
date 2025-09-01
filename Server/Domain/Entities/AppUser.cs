namespace Domain.Entities
{
    public class AppUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Rating> Ratings { get; set; }
    }
}
