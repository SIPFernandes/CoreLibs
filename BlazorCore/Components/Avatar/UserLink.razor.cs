using BlazorCore.Areas.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Avatar
{
    public partial class UserLink : ComponentBase
    {
        [Parameter, EditorRequired]
        public string UserId { get; set; } = default!;
        [Inject]
        public IUserCoreService UserCoreService { get; set; } = default!;
        private string _userName = default!;
        private string _userLink = default!;

        protected override void OnInitialized()
        {            
            _userLink = UserCoreService.GetUserLink(UserId);

            _userName = UserCoreService.GetUserName(UserId);
        }
    }
}