using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Rating
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }
        public Guid AppUserId { get; set; }
        public virtual Student Student { get; set; }
        public string Description { get; set; }
        public Guid? ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }
    }
}