using System.Collections;
using DotNetCore.SendEmail.Models;

namespace DotNetCore.SendEmail.Services
{
    public interface IEmailSenderService
    {
        public Task SendEmailAsync(string email, string subject,
            PlaceholdersEmailModel placeholdersEmailModel, bool dispose = true);
        public Task SendMultipleEmailAsync(IEnumerable emails, string subject,
            PlaceholdersEmailModel placeholdersEmailModel, bool dispose = true);
    }
}
