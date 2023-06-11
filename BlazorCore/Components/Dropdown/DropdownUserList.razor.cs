using BlazorCore.Areas.Interfaces;
using DotNetCore.Helpers;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Dropdown
{
    public partial class DropdownUserList : ComponentBase
    {
        [Parameter]
        public bool ExcludeCurrentUser { get; set; }
        [Parameter]
        public HashSet<string> SelectedUsersId { get; set; } = default!;
        [Parameter]
        public EventCallback<string> UserSelectedChanged { get; set; }        
        [Parameter]
        public bool SelectOnlyOne { get; set; }
        [Inject]
        IUserCoreService UserCoreService { get; set; } = default!;
        private IEnumerable<UserListItemModel> UserItemList { get; set; } = default!;
        private IEnumerable<UserListItemModel> FilteredUserItemList { get; set; } = default!;
        private readonly string OnlyOneException = "Select Only One is true but you are passing more then one selected User Id.";

        private string SearchExpression = string.Empty;

        protected override void OnInitialized()
        {
            if (SelectedUsersId is null)
            {
                SelectedUsersId = new HashSet<string>();
            }
            else if (SelectOnlyOne && SelectedUsersId.Count > 1)
            {
                throw new Exception(OnlyOneException);
            }

            UserItemList = UserCoreService.GetUsers(SelectedUsersId, ExcludeCurrentUser);

            FilteredUserItemList = UserItemList;
        }

        private void OnSearchChange(string search)
        {
            SearchExpression = search;

            if(SearchExpression == string.Empty)
            {
                FilteredUserItemList = UserItemList;
            }
            else
            {
                FilteredUserItemList = UserItemList
                .Where(x => UsersHelper.SearchMembers(x.FirstName, x.LastName, SearchExpression));
            }
        }

        private async Task OnUserSelect(UserListItemModel user)
        {
            if (user.IsSelected)
            {
                SelectedUsersId.Remove(user.Id);

                user.IsSelected = false;
            }
            else
            {
                if (SelectOnlyOne)
                {
                    SelectedUsersId.Clear();
                }

                SelectedUsersId.Add(user.Id);

                user.IsSelected = true;
            }

            FilteredUserItemList = FilteredUserItemList
                .OrderByDescending(x => x.IsSelected)
                .ThenBy(x => x.FirstName);

            await UserSelectedChanged.InvokeAsync(user.Id);
        }        

        public class UserListItemModel
        {
            public string Id { get; init; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public bool IsSelected { get; set; }

            public UserListItemModel(string id) 
            { 
                Id = id;                
            }
        }
    }
}
