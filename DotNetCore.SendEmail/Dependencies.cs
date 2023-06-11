using DotNetCore.SendEmail.Consts;
using DotNetCore.SendEmail.HostedServices.QueuedBackgroundTasksService;
using DotNetCore.SendEmail.HostedServices.QueuedBackgroundTasksService.EmailQueuedHostedService;
using DotNetCore.SendEmail.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.SendEmail
{
    public static class Dependencies
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            
            services.AddHostedService<EmailQueuedHostedService>();
            
            
            services.AddSingleton<IBackgroundTaskQueue>(ctx =>
            {
                var value = configuration
                    .GetSection(ConfigurationConst.BackgroundServiceQueueCapacity)
                    .Value;

                if (!int.TryParse(value, out var queueCapacity))
                    queueCapacity = 100;

                return new BackgroundTaskQueue(queueCapacity);
            });


            services.AddSingleton<IPlaceholdersEmailService, PlaceholdersEmailService>();
        }
    }
}