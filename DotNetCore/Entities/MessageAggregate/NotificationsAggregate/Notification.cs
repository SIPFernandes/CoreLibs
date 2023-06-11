using DotNetCore.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore.Entities.MessageAggregate.NotificationsAggregate
{
    public class Notification : BaseEntity, IAggregateRoot
    {
        [Required]
        public string Text { get; private set; }
        public int NotificationObjectId { get; private set; }
        public string Type { get; private set; }
        public string? DesinationUsersRef { get; private set; }

        public Notification(string text, string type, string creatorId,
            string? desinationUsersRef = null, int notificationObjectId = 0) : base(creatorId)
        {
            Text = text;
            Type = type;
            NotificationObjectId = notificationObjectId;
            DesinationUsersRef = desinationUsersRef;
        }
    }
}
