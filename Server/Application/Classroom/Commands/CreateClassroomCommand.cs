using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Classroom.Commands
{
    public record CreateClassroomCommand(string Name):IRequest<Result<bool>>;
    public class CreateClassroomCommandHandler : IRequestHandler<CreateClassroomCommand, Result<bool>>
    {
        private readonly IClassroomService _service;
        public CreateClassroomCommandHandler(IClassroomService service)
        {
            _service = service;
        }

        public async Task<Result<bool>> Handle(CreateClassroomCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.CreateClassroomAsync(request.Name);
            return result;
        }
    }
}