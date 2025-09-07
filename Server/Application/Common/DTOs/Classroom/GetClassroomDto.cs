using Application.Common.DTOs.User;

namespace Application.Common.DTOs.Classroom
{
    public class GetClassroomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GetUserDto> Students { get; set; } = new();
        public GetUserDto Teacher { get; set; } = new();
    }
}