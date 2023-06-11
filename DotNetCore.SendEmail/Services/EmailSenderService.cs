using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using DotNetCore.SendEmail.Consts;
using DotNetCore.SendEmail.HostedServices.QueuedBackgroundTasksService;
using DotNetCore.SendEmail.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DotNetCore.SendEmail.Services
{
    public class EmailSenderService : IEmailSenderService, IDisposable
    {
        private readonly SmtpClient _smtp;
        private readonly string _fromEmail;
        private readonly IPlaceholdersEmailService _placeholdersEmailService;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly CancellationToken _cancellationToken;
        private readonly List<MailMessage> _disposeMailList;        

        public EmailSenderService(IConfiguration configuration,
            IPlaceholdersEmailService placeholdersEmailService,
            IBackgroundTaskQueue taskQueue,
            IHostApplicationLifetime applicationLifetime)
        {
            _placeholdersEmailService = placeholdersEmailService;
            _taskQueue = taskQueue;
            _cancellationToken = applicationLifetime.ApplicationStopping;

            _fromEmail = configuration.GetValue<string>(ConfigurationConst.EmailConst.EmailSender);

            _smtp = new SmtpClient
            {
                Host = configuration.GetValue<string>(ConfigurationConst.EmailConst.Host),
                Port = configuration.GetValue<int>(ConfigurationConst.EmailConst.Port),
                EnableSsl = true,                
                Credentials = new NetworkCredential
                {
                    UserName = _fromEmail,
                    Password = configuration.GetValue<string>(ConfigurationConst.EmailConst.EmailPassword)
                }
            };

            _disposeMailList = new();                        
        }

        public async Task SendEmailAsync(string email, string subject,
            PlaceholdersEmailModel placeholdersEmailModel, bool dispose = true)
        {            
            var message = CreateMailMessage(subject, placeholdersEmailModel);

            message.Bcc.Add(new MailAddress(email));

            await _taskQueue.QueueBackgroundWorkItemAsync(async (x) =>
                await SendEmail(message, dispose, x));
        }

        public async Task SendMultipleEmailAsync(IEnumerable emails, string subject,
            PlaceholdersEmailModel placeholdersEmailModel, bool dispose = true)
        {
            var message = CreateMailMessage(subject, placeholdersEmailModel);

            foreach (string email in emails)
            {
                message.Bcc.Add(new MailAddress(email));
            }                        

            await _taskQueue.QueueBackgroundWorkItemAsync(async (x) =>
                await SendEmail(message, dispose, x));
        }

        public void Dispose()
        {            
            _smtp?.Dispose();

            _disposeMailList.ForEach(mail => mail.Dispose());            
        }        

        private async Task SendEmail(MailMessage message, bool dispose, CancellationToken token)
        {                                 
            if (!token.IsCancellationRequested)
            {
                await _smtp.SendMailAsync(message, _cancellationToken);

                DisposeMailMessage(message, dispose);
            }            
        }      

        private MailMessage CreateMailMessage(string subject,
            PlaceholdersEmailModel placeholdersEmailModel)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_fromEmail),

                Subject = subject,

                IsBodyHtml = true
            };

            var tempHtmlMessage = _placeholdersEmailService
                .ReplacePlaceholders(placeholdersEmailModel.EmailModel, placeholdersEmailModel.ResourceManager);

            mailMessage.AlternateViews.Add(GetAlternativeView(tempHtmlMessage.ToString(),
                placeholdersEmailModel.LinkedResources.Values));

            return mailMessage;
        }

        private static AlternateView GetAlternativeView(string htmlBody, IEnumerable<LinkedResource> resources)
        {
            var alternateView = AlternateView.CreateAlternateViewFromString(
                htmlBody, null, MediaTypeNames.Text.Html);

            foreach (var r in resources)
            {
                alternateView.LinkedResources.Add(r);
            }

            return alternateView;
        }

        private void DisposeMailMessage(MailMessage mail, bool dispose)
        {
            if (dispose)
            {
                mail.Dispose();
            }
            else
            {
                _disposeMailList.Add(mail);
            }            
        }        
    }
}
