
using Application.Common;
using MediatR;

namespace Application.User.Command
{
    public record DeleteUserCommand(string Password): IRequest<Result<bool>>;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly Common.Interfaces.IUserService _service;
        public DeleteUserCommandHandler(Common.Interfaces.IUserService service)
        {
            _service = service;
        }
        public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _service.DeleteUser(request.Password);
            return result;
        }
    }


}
