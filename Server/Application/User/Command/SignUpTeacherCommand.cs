using Application.Common;
using Application.Common.DTOs.User;
using Application.Common.Interfaces;
using MediatR;

public record SignUpStudentCommand(string Email, string Name,string Surname, string Password, string PasswordRepeated) : IRequest<Result<TokenDto>>;

public class SignUpStudentHandler : IRequestHandler<SignUpStudentCommand, Result<TokenDto>>
{
    private readonly IUserService _service;
    public SignUpStudentHandler(IUserService service)
    {
        _service = service;
    }

    public async Task<Result<TokenDto>> Handle(SignUpStudentCommand command, CancellationToken cancellationToken)
    {
        var result = await _service.SignUpStudent(command.Email,command.Name,command.Surname,command.Password,command.PasswordRepeated);
        return result;
    }
}