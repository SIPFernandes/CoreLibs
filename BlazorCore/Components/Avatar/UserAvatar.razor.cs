using BlazorCore.Areas.Interfaces;
using BlazorCore.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Avatar
{
    public partial class UserAvatar : ComponentBase
    {
        [Parameter, EditorRequired]
        public string UserId { get; set; } = default!;
        [Parameter]
        public int AvatarSize { set; get; } = 33;
        [Inject]
        IUserCoreService UserCoreService { get; set; } = default!;
        private string _userId = default!;
        private AvatarModel? AvatarModel;

        protected override void OnInitialized()
        {
            SetUserAvatar();
        }

        protected override void OnParametersSet()
        {
            if (_userId != UserId)
            {
                SetUserAvatar();
            }
        }

        private void SetUserAvatar()
        {
            _userId = UserId;

            AvatarModel = UserCoreService.GetUserAvatar(UserId);
        }
    }
}