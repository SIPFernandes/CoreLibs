using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.EmptyState
{
    public partial class EmptyState : ComponentBase
    {
        [Parameter]
        public string IconSvg { get; set; } = default!;
        [Parameter]
        public string Title { get; set; } = default!;
        [Parameter]
        public string? Description { get; set; }
        [Parameter]
        public RenderFragment? MoreInfo { get; set; }
    }
}
