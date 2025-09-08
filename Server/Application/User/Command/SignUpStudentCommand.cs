using Application.Common;
using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using MediatR;

public record SignUpTeacherCommand(string Email, string Name,string Surname, string Password, string PasswordRepeated) : IRequest<Result<TokenDto>>;

public class SignUpTeacherHandler : IRequestHandler<SignUpTeacherCommand, Result<TokenDto>>
{
    private readonly IUserService _service;
    public SignUpTeacherHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Result<TokenDto>> Handle(SignUpTeacherCommand command, CancellationToken cancellationToken)
    {
        var result = await _service.SignUpTeacher(command.Email,command.Name,command.Surname,command.Password,command.PasswordRepeated);
        return result;
    }
}