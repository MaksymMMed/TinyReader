using Application.Common.DTOs.Classroom;

namespace Application.Common.Interfaces
{
    public interface IClassroomService
    {
        public Task<Result<bool>> AddUserToClassroomAsync(Guid classroomId, Guid userId);
        public Task<Result<bool>> RemoveUserFromClassroomAsync(Guid classroomId, Guid userId);
        public Task<Result<bool>> CreateClassroomAsync(string name);
        public Task<Result<bool>> DeleteClassroomAsync(Guid classroomId);
        public Task<Result<bool>> UpdateClassroomAsync(Guid classroomId, string name);
        public Task<Result<GetClassroomDto>> GetClassroomByIdAsync(Guid classroomId);

    }
}
