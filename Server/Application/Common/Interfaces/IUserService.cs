using Application.Common.DTOs.User;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<Result<bool>> SignUp(AppUser user, string password, string passwordRepeated);
        Task<Result<TokenDto>> SignIn(string Email, string password);
    }
}
