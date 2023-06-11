using DotNetCore.Interfaces;

namespace DotNetCore.Entities.MessageAggregate.NotificationsAggregate
{
    public class NotificationsLastVisit : IAggregateRoot
    {
        public int NotificationObjectId { get; private set; }
        public string UserId { get; set; }
        public DateTime LastVisitDate { get; set; }

        public NotificationsLastVisit(string userId, DateTime lastVisitDate, int notificationObjectId = 0) 
        { 
            UserId = userId;
            LastVisitDate = lastVisitDate;
            NotificationObjectId = notificationObjectId;
        }
    }
}
