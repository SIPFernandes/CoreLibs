using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Consts;
using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorCore.Components.CommentFeed
{
    public partial class FeedUsersList : ComponentBase
    {        
        [Parameter]
        public EventCallback<AppUserModel> UserSelected { get; set; }
        [JSInvokable]
        public void OnSelectedIndexChanged(int index) => UpdateCurrentIndex(index);
        [Inject]
        ICommentFeedService CommentFeedService { get; set; } = default!;
        [Inject]
        IJSInteropService JsService { get; set; } = default!;
        private List<AppUserModel> FilteredList { get; set; } = default!;
        private IEnumerable<AppUserModel> Users { get; set; } = default!;
        private ElementReference UsersListRef { get; set; }
        private int CurrentIndex = 0;
        private DotNetObjectReference<FeedUsersList>? Reference;

        protected override void OnInitialized()
        {
            Users = CommentFeedService.Users.Where(x => x.Id != CommentFeedService.CurrentUserId);

            FilteredList = Users.ToList();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Reference ??= DotNetObjectReference.Create(this);

                await JsService.SubscribeToArrowsMovement(Reference, 
                    UsersListRef, CommentsConsts.UserItemClass,
                    ColorsConsts.Grey);
            }
        }

        public async Task SelectUser()
        {
            if(FilteredList.Count > 0)
            {
                var user = FilteredList[CurrentIndex];

                if (user != null)
                {
                    await UserSelected.InvokeAsync(user);
                }
            }
            
        }

        public void OnSearchChange(string searchExpression)
        {
            FilteredList = Users
                .Where(x => x.Email.Contains(searchExpression, StringComparison.CurrentCultureIgnoreCase) ||
                    x.FirstName != null && x.FirstName.Contains(searchExpression, StringComparison.CurrentCultureIgnoreCase) ||
                    x.LastName != null && x.LastName.Contains(searchExpression, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            StateHasChanged();
        }

        private void UpdateCurrentIndex(int index)
        {
            CurrentIndex = index;
        }
    }
}
