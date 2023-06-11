using BlazorCore.Data.Models;
using static BlazorCore.Components.Dropdown.DropdownUserList;

namespace BlazorCore.Areas.Interfaces
{
    public interface IUserCoreService
    {
        public string GetCurrentUserId();
        public string GetCurrentUserRole();
        public string GetUserName(string userId);
        public string GetUserLink(string userId);
        public AvatarModel? GetUserAvatar(string userId);
        public IEnumerable<UserListItemModel> GetUsers(HashSet<string> selectedUsersIds, bool includeCurrentUser = true);
    }
}
