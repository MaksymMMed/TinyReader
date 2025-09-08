using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Classroom.Commands
{
    public record CreateClassroomRequest(string Name):IRequest<Result<bool>>;
    public class CreateClassroomCommandHandler : IRequestHandler<CreateClassroomRequest, Result<bool>>
    {
        private readonly IClassroomService _service;
        public CreateClassroomCommandHandler(IClassroomService service)
        {
            _service = service;
        }

        public async Task<Result<bool>> Handle(CreateClassroomRequest request, CancellationToken cancellationToken)
        {
            var result = await _service.CreateClassroomAsync(request.Name);
            return result;
        }
    }
}