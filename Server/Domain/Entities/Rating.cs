namespace Domain.Entities
{
    public class Rating
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Description { get; set; }
    }
}
