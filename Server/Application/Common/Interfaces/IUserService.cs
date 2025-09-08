using Application.Common.DTOs.User;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<Result<TokenDto>> SignIn(string email, string password);
        Task<Result<TokenDto>> SignUpStudent(string email, string name, string surname, string password, string passwordRepeated);
        Task<Result<TokenDto>> SignUpTeacher(string email, string name, string surname, string password, string passwordRepeated);
    }
}
