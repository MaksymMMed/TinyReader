using Application.Common;
using Application.Common.DTOs.Classroom;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Classroom.Queries
{
    public record GetClassroomDetailsQuery(Guid ClassroomId) : IRequest<Result<GetClassroomDto>>;

    public class GetClassroomDetailsQueryHandler : IRequestHandler<GetClassroomDetailsQuery, Result<GetClassroomDto>>
    {
        private readonly IClassroomService _service;
        public GetClassroomDetailsQueryHandler(IClassroomService service)
        {
            _service = service;
        }
        public async Task<Result<GetClassroomDto>> Handle(GetClassroomDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _service.GetClassroomByIdAsync(request.ClassroomId);
            return result;
        }
    }
}
