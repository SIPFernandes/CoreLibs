using BlazorCore.Areas.Interfaces;
using DotNetCore.Entities.MessageAggregate.NotificationsAggregate;
using DotNetCore.Helpers;
using DotNetCore.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace BlazorCore.Components.Notifications
{
    public partial class NotificationsList : ComponentBase
    {
        [Parameter]
        public Notification? NewNotification { get; set; }
        [Parameter]
        public DateTime? LastViewed { get; set; }
        [Parameter]
        public Expression<Func<Notification, bool>>? Predicate { get; set; }
        [Parameter]
        public RenderFragment<Notification>? Template { get; set; }
        [Parameter]
        public EventCallback<(bool, DateTime?)> ItemLoaded { get; set; }
        [Inject]
        IGetItemsService<Notification> GetItemsService { get; set; } = default!;
        [Inject]
        IUserCoreService UserCoreService { get; set; } = default!;
        private List<Notification> _notifications = default!;
        private Expression<Func<Notification, bool>>? _predicate;
        private Expression<Func<Notification, bool>> _baseExpression = default!;
        private Expression<Func<Notification, bool>> _expression = default!;
        private Notification? _newNotification;
        private bool NewSet;
        private bool ViewedSet;

        public async Task GetMore()
        {
            await GetItemsService.GetMoreItems(_notifications);
        }

        protected override async Task OnInitializedAsync()
        {
            var currentUserId = UserCoreService.GetCurrentUserId();

            var currentUserRole = UserCoreService.GetCurrentUserRole();

            _baseExpression = x => x.DesinationUsersRef == null || x.DesinationUsersRef.Contains(currentUserId) ||
             x.DesinationUsersRef == currentUserRole;

            await GetNotifications();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (_predicate != Predicate)
            {
                await GetNotifications();
            }            
            else if (_newNotification != NewNotification)
            {
                _newNotification = NewNotification;

                if (_newNotification != null)
                {
                    _notifications.Remove(_notifications[^1]);

                    _notifications = _notifications.Prepend(_newNotification)
                        .ToList();
                }                
            }
        }

        private async Task GetNotifications()
        {
            _predicate = Predicate;

            NewSet = false; ViewedSet = false;

            if (_predicate != null)
            {
                _expression = ExpressionHelper.AndExpression(_baseExpression, _predicate);
            }
            else
            {
                _expression = _baseExpression;
            }

            _notifications = await GetItemsService.GetRecentItems(_expression);

            if (ItemLoaded.HasDelegate)
            {
                var recentNotifDate = _notifications.FirstOrDefault()?.CreatedAt;

                await ItemLoaded.InvokeAsync((GetItemsService.ItemsLoaded, recentNotifDate));
            }
        }
    }
}
