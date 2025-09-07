using Application.Common;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

public record SignUpCommand(string Email, string Name,string Surname, string Password, string PasswordRepeated) : IRequest<Result<bool>>;

public class SignUpHandler : IRequestHandler<SignUpCommand, Result<bool>>
{
    private readonly IUserService _service;
    public SignUpHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Result<bool>> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            Name = command.Name,
            Surname = command.Surname,
            Email = command.Email,
        };

        var result = await _service.SignUp(user, command.Password,command.PasswordRepeated);
        return result;
    }
}
