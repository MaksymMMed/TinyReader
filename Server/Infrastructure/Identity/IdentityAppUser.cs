using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentityAppUser:IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Guid? ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }
    }
}
