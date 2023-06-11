using BlazorCore.Areas.Interfaces;
using DotNetCore.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.Reactions
{
    public partial class ReactionsBase : ComponentBase
    {
        [Parameter, EditorRequired]
        public string UserId { get; set; } = default!;
        [Parameter, EditorRequired]
        public ReactedObject ReactedObj { get; set; } = default!;
        [Parameter, EditorRequired]
        public ReactionsOptions Options { get; set; } = default!;
        [Parameter]
        public EventCallback OnReacting { get; set; }
        [Parameter]
        public bool Disabled { get; set; }
        [Inject]
        IReactedObjService ReactedObjService { get; set; } = default!;
        private string? MyReaction;

        protected override void OnInitialized()
        {
            MyReaction = ReactedObj.GetUserReaction(UserId);
        }

        private async Task ReactionClick(string reaction)
        {
            if (!Disabled)
            {
                var reacted = await ReactedObjService.ReactUnreact(ReactedObj, reaction, UserId);

                if (reacted && OnReacting.HasDelegate)
                {
                    await OnReacting.InvokeAsync();
                }

                MyReaction = reacted ? reaction : null;
            }
        }
    }
}

public class ReactionsOptions
{
    public bool OnlyLikes { get; set; }
    public string? LikesLabel { get; set; }
}