using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Reactions
{
    public partial class ReactionsDropdown : ComponentBase
    {
        [Parameter]
        public string? SelectedReaction { get; set; }
        [Parameter, EditorRequired]
        public EventCallback<string?> SelectedReactionChanged { get; set; }
        [Parameter]
        public bool Disabled { get; set; }
    }
}
