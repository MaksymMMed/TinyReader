using Application.Common;
using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using MediatR;

namespace Application.User.Query
{
    public record SignInQuery(string Email, string Password) : IRequest<Result<TokenDto>>;

    public class SignInHandler : IRequestHandler<SignInQuery, Result<TokenDto>>
    {
        private readonly IUserService _service;
        public SignInHandler(IUserService service)
        {
            _service = service;
        }

        public async Task<Result<TokenDto>> Handle(SignInQuery query, CancellationToken cancellationToken)
        {
            var result = await _service.SignIn(query.Email,query.Password);
            return result;
        }
    }
}
