using BlazorCore.Areas.Interfaces;
using DotNetCore.Entities.MessageAggregate.CommentAggregate;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.CommentFeed
{
    public partial class CommentReply : ComponentBase
    {
        [Parameter, EditorRequired]
        public CommentReplied Reply { set; get; } = default!;
        [Parameter]
        public EventCallback RemoveReply { set; get; }
        [Parameter]
        public bool Dark { set; get; }
        [Inject]
        public ICommentFeedService CommentFeedService { get; set; } = default!;
        private string ReplyUserName { get; set; } = default!;

        protected override void OnInitialized()
        {
            ReplyUserName = CommentFeedService.GetUserName(Reply.CreatorId);
        }

        private async Task RemoveReplyEvent()
        {
            await RemoveReply.InvokeAsync();
        }
    }
}