using TimersTimer = System.Timers.Timer;
using Microsoft.Extensions.Logging;

namespace DotNetCore.SendEmail.HostedServices.QueuedBackgroundTasksService.EmailQueuedHostedService
{
    public class EmailQueuedHostedService : QueuedHostedService
    {
        private readonly ILogger<EmailQueuedHostedService> _logger;
        private readonly TimersTimer _timer;
        private int MailsSent = 0;
        private const int MailsSentInMinute = 30;

        public EmailQueuedHostedService(IBackgroundTaskQueue taskQueue,
            ILogger<EmailQueuedHostedService> logger) : base(taskQueue, logger)
        {
            _logger = logger;
            _timer = new TimersTimer(60 * 1000);
            _timer.Elapsed += (sender, e) => MailsSent = 0;
            _timer.Start();
        }

        protected override async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (MailsSent < MailsSentInMinute)
                {
                    var workItem = await TaskQueue
                        .DequeueAsync(stoppingToken);

                    try
                    {
                        await workItem(stoppingToken);

                        MailsSent++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Error occurred executing {WorkItem}.", nameof(workItem));
                    }
                }
            }
        }
    }
}
