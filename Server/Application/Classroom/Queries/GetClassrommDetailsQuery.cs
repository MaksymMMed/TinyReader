using Application.Common;
using Application.Common.DTOs.Classroom;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Classroom.Queries
{
    public record GetClassrommDetailsQuery(Guid ClassroomId) : IRequest<Result<GetClassroomDto>>;

    public class GetClassrommDetailsQueryHandler : IRequestHandler<GetClassrommDetailsQuery, Result<GetClassroomDto>>
    {
        private readonly IClassroomService _service;
        public GetClassrommDetailsQueryHandler(IClassroomService service)
        {
            _service = service;
        }
        public async Task<Result<GetClassroomDto>> Handle(GetClassrommDetailsQuery request, CancellationToken cancellationToken)
        {
            var result = await _service.GetClassroomByIdAsync(request.ClassroomId);
            return result;
        }
    }
}
