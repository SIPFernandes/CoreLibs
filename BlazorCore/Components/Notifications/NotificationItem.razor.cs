using DotNetCore.Entities.MessageAggregate.NotificationsAggregate;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Notifications
{
    public partial class NotificationItem : ComponentBase
    {
        [Parameter, EditorRequired]
        public Notification Notification { get; set; } = default!;
    }
}
