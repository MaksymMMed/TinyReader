using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Classroom.Commands
{
    public record UpdateClassroomCommand(Guid ClassroomId, string Name) : IRequest<Result<bool>>;
    public class UpdateClassroomCommandHandler : IRequestHandler<UpdateClassroomCommand, Result<bool>>
    {
        private readonly IClassroomService _service;
        public UpdateClassroomCommandHandler(IClassroomService service)
        {
            _service = service;
        }
        public async Task<Result<bool>> Handle(UpdateClassroomCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.UpdateClassroomAsync(request.ClassroomId, request.Name);
            return result;
        }
    }
}
