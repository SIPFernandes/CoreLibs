using BlazorCore.Areas.Interfaces;
using BlazorCore.Components.TabSet;
using BlazorCore.Data.Consts;
using DotNetCore.Entities.MessageAggregate;
using DotNetCore.Entities.MessageAggregate.NotificationsAggregate;
using DotNetCore.Helpers;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using System.Text.Json;
using DotNetCoreAppConst = DotNetCore.Consts.AppConst;

namespace BlazorCore.Components.Notifications
{
    public partial class NotificationBell : ComponentBase, IDisposable
    {        
        [Parameter]
        public DateTime? LastViewed { get; set; }
        [Parameter]
        public RenderFragment<Notification>? Template { get; set; }        
        [Parameter]
        public List<TabSetItem>? TabItems { get; set; }
        [Parameter]
        public string Icon { get; set; } = IconsConst.Notification;
        [Parameter]
        public EventCallback NotificationsClosed { get; set; }
        [Inject]
        IHubClientBaseService HubClientBaseService { get; set; } = default!;
        [Inject]
        NavigationManager NavigationManager { get; set; } = default!;        
        private Expression<Func<Notification, bool>>? _predicate;
        private TabSetItem? _selectedTabItem;
        private Notification? _newNotification;
        private bool _hasNews;
        private bool _hasMoreItems;        

        public void Dispose()
        {
            HubClientBaseService.UnsubscribeToMethod(DotNetCoreAppConst.RealTime.NotificationsFeed);
        }

        protected override void OnInitialized()
        {
            HubClientBaseService.SubscribeToMethod(NotificationReceived, DotNetCoreAppConst.RealTime.NotificationsFeed);

            if (TabItems != null && TabItems.Count > 0)
            {
                SelectedTabChanged(TabItems[0]);                
            }            
        }              

        private async Task OnDropdownClose()
        {
            _hasNews = false;

            if (NotificationsClosed.HasDelegate)
            {
                await NotificationsClosed.InvokeAsync();
            }
        }

        private void SelectedTabChanged(TabSetItem newTab)
        {
            _selectedTabItem = newTab;

            _predicate = x => x.NotificationObjectId == _selectedTabItem.Value;
        }

        private void ViewAllNotifications()
        {
            var url = _selectedTabItem is null ? AppConst.NotificationsUrlConst.Notifications :
                AppConst.NotificationsUrlConst.Notifications + $"/{_selectedTabItem.Value}";

            NavigationManager.NavigateTo(url);
        }

        private Task NotificationReceived(HubMessage message)
        {
            _hasNews = true;

            _newNotification = message.GetObj<Notification>();

            InvokeAsync(StateHasChanged);

            return Task.CompletedTask;
        }  
        
        private void OnItemsLoaded((bool loaded, DateTime? lastNotifDate) data) 
        {
            _hasMoreItems = !data.loaded;

            _hasNews = data.lastNotifDate > LastViewed;
        }
    }
}
