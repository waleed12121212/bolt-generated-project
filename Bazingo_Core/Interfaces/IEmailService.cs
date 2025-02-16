using System.Threading.Tasks;

namespace Bazingo_Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task SendEmailConfirmationAsync(string email, string confirmationLink);
        Task SendPasswordResetAsync(string email, string resetLink);
        Task Send2FACodeAsync(string email, string code);
    }
}
