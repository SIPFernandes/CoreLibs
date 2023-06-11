using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.User
{
    public partial class UserTooltipTrigger : ComponentBase
    {
        [Parameter, EditorRequired]
        public string UserId { set; get; } = default!;
        [Parameter, EditorRequired]
        public RenderFragment UserPopup { set; get; } = default!;
        [Parameter]
        public int AvatarSize { set; get; } = 32;
        [Parameter]
        public int TooltipWidth { set; get; } = 240;
    }
}
