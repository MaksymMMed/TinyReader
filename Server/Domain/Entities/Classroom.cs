using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities
{
    public class Classroom
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Exercise> Exercises { get; set; }
        public ICollection<Student> Students { get; set; }
        public Guid? TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}