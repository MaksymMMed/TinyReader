using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Classroom.Commands
{
    public record RemoveStudentFromClassroomCommand(Guid studentId,Guid classroomId):IRequest<Result<bool>>;
    public class RemoveStudentFromClassroomHandler : IRequestHandler<RemoveStudentFromClassroomCommand, Result<bool>>
    {
        private readonly IClassroomService _service;
        public RemoveStudentFromClassroomHandler(IClassroomService service)
        {
            _service = service;
        }

        public Task<Result<bool>> Handle(RemoveStudentFromClassroomCommand request, CancellationToken cancellationToken)
        {
            var result = _service.RemoveUserFromClassroomAsync(request.classroomId, request.studentId);
            return result;
        }
    }
}
