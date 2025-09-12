namespace Application.Common.Utils
{
    public interface IEmailSender
    {
        public Task SendConfirmationEmailAsync(string email, string message);
        public Task SendResetPasswordEmailAsync(string email, string message);
    }
}
