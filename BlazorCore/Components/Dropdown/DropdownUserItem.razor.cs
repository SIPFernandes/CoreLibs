using Microsoft.AspNetCore.Components;
using static BlazorCore.Components.Dropdown.DropdownUserList;

namespace BlazorCore.Components.Dropdown
{
    public partial class DropdownUserItem : ComponentBase
    {
        [Parameter]
        public UserListItemModel UserItem { get; set; }
        [Parameter]
        public EventCallback<UserListItemModel> UserItemChanged { get; set; }
    }
}
