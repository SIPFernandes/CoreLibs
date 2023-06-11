using DotNetCore.Entities.MessageAggregate.NotificationsAggregate;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DotNetCore.Interfaces
{
    public interface INotificationService : IItemService<Notification>
    {
        public Task Insert(Notification obj);
        public Task UpdateNotification(Expression<Func<Notification, bool>> expression,
            Expression<Func<SetPropertyCalls<Notification>, SetPropertyCalls<Notification>>> valueExpression);
        public Task<DateTime> SetUserLastVisit(string userId, bool newDate);
        public Task<DateTime?> GetUserNotificationLastVisit(string userId);        
    }
}
