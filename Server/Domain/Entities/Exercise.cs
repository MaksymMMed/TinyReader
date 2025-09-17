using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Exercise
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid? ClassroomId { get; set; }
        public virtual Classroom Classroom { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}