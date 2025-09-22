using Application.Common.DTOs.User;

namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<Result<TokenDto>> SignIn(string email, string password);
        Task<Result<TokenDto>> SignUpStudent(string email, string name, string surname, string password, string passwordRepeated);
        Task<Result<TokenDto>> SignUpTeacher(string email, string name, string surname, string password, string passwordRepeated);
        Task<Result<bool>> DeleteUser(string password);
        Task<Result<string>> GenerateConfirmationToken(string email);
        Task<Result<string>> GenerateResetPasswordToken(string email);
        Task SendConfirmationEmail(string email, string confirmationLink);
        Task SendResetPasswordEmail(string email, string confirmationLink);
        Task<Result<string>> ResetPassword(string email, string resetToken, string newPassword, string confirmPassword);
        Task<Result<string>> ConfirmEmail(string email, string confirmationToken);
        Task<Result<string>> ChangePassword(string oldPassword, string newPassword);
        Task<Result<string>> ChangeUserName(string newName, string newSurname);
        Task<Result<string>> ChangeEmail(string newEmail);

    }
}
