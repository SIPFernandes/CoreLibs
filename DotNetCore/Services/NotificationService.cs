using DotNetCore.Consts;
using DotNetCore.Entities.MessageAggregate;
using DotNetCore.Entities.MessageAggregate.NotificationsAggregate;
using DotNetCore.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using static DotNetCore.Hubs.BaseHub;

namespace DotNetCore.Services
{
    public class NotificationService : INotificationService
    {        
        private readonly INotificationsRepository _repository;
        private readonly IHubBaseService _hubService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(INotificationsRepository repository,
            IHubBaseService hubService,
            ILogger<NotificationService> logger)
        {
            _hubService = hubService;
            _repository = repository;            
            _logger = logger;
        }

        public async Task<List<Notification>> GetItems(Expression<Func<Notification, bool>>? expression = null, int skip = 0, int take = 20)
        {
            return await _repository.GetNItemsWhere(null, expression, skip, take);
        }

        public Task<List<W>> GetItems<W>(Expression<Func<Notification, W>> selector, Expression<Func<Notification, bool>>? expression = null, int skip = 0, int take = 20)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetItemsOrdered<Z>(Expression<Func<Notification, Z>> orderedBy, bool descending, Expression<Func<Notification, bool>>? expression = null, int skip = 0, int take = 20)
        {
            throw new NotImplementedException();
        }

        public Task<List<W>> GetItemsOrdered<W, Z>(Expression<Func<Notification, W>> selector, Expression<Func<Notification, Z>> orderedBy, Expression<Func<Notification, bool>>? expression = null, int skip = 0, int take = 20)
        {
            throw new NotImplementedException();
        }

        public async Task<DateTime?> GetUserNotificationLastVisit(string userId)
        {
            return await _repository.GetUserNotificationLastVisit(userId);
        }

        public async Task Insert(Notification obj)
        {
            await _repository.Insert(obj);

            await SendHubMsg(obj);
        }

        public async Task<DateTime> SetUserLastVisit(string userId, bool newDate)
        {
            return await _repository.SetUserLastVisit(userId, newDate);
        }

        public async Task UpdateNotification(Expression<Func<Notification, bool>> expression,
            Expression<Func<SetPropertyCalls<Notification>, SetPropertyCalls<Notification>>> valueExpression)
        {
            await _repository.UpdateMultipleLeafType(expression, valueExpression);
        }

        private async Task SendHubMsg(Notification notification)
        {
            if (notification.DesinationUsersRef is null)
            {
                var msg = new HubMessage(notification, notification.Type, notification.CreatorId);

                await _hubService.SendMessage(msg, AppConst.RealTime.NotificationsFeed);
            }
            else
            {
                var destinations = notification.DesinationUsersRef.Split(';');

                foreach (var dest in destinations)
                {
                    if (!string.IsNullOrEmpty(dest))
                    {
                        if (dest == AppConst.Policies.Admin)
                        {
                            var groupMsg = new BaseGroupMsgModel(AppConst.Policies.Admin,
                                notification, notification.Type, notification.CreatorId);

                            await _hubService.SendGroupMessage(groupMsg, AppConst.RealTime.NotificationsFeed);
                        }
                        else
                        {
                            var dm = new BaseDirectMsgModel(dest, notification,
                                notification.Type, notification.CreatorId);

                            await _hubService.SendDirectMessage(dm, AppConst.RealTime.NotificationsFeed);
                        }
                    }
                }
            }
        }
    }
}
