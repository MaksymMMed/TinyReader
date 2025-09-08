using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Classroom.Commands
{
    public record DeleteClassroomCommand(Guid ClassroomId) : IRequest<Result<bool>>;

    public class DeleteClassroomCommandHandler : IRequestHandler<DeleteClassroomCommand, Result<bool>>
    {
        private readonly IClassroomService _service;
        public DeleteClassroomCommandHandler(IClassroomService service)
        {
            _service = service;
        }
        public async Task<Result<bool>> Handle(DeleteClassroomCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.DeleteClassroomAsync(request.ClassroomId);
            return result;
        }
    }
}
