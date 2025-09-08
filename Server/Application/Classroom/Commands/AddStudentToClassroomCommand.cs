using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Classroom.Commands
{
    public record AddStudentToClassroomCommand(Guid studentId,Guid classroomId): IRequest<Result<bool>>;
    public class AddStudentToClassroomCommandHandler : IRequestHandler<AddStudentToClassroomCommand, Result<bool>>
    {
        private readonly IClassroomService _service;
        public AddStudentToClassroomCommandHandler(IClassroomService service)
        {
            _service = service;
        }
        public async Task<Result<bool>> Handle(AddStudentToClassroomCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.AddUserToClassroomAsync(request.classroomId, request.studentId);
            return result;
        }
    }
}
