using DotNetCore.Entities.MessageAggregate.NotificationsAggregate;

namespace DotNetCore.Interfaces
{
    public interface INotificationsRepository : IRepository<Notification>
    {
        public Task<DateTime> SetUserLastVisit(string userId, bool newDate);
        public Task<DateTime?> GetUserNotificationLastVisit(string userId);        
    }
}
