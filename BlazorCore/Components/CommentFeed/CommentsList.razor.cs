using BlazorCore.Data.Models.FeedModels;
using DotNetCore.Entities.MessageAggregate.CommentAggregate;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.CommentFeed
{
    public partial class CommentsList<T> : ComponentBase where T : Comment
    {
        [Parameter, EditorRequired]
        public FeedDataModel FeedData { get; set; } = default!;
        [Parameter, EditorRequired]
        public List<T>? Comments { get; set; }        
        [Parameter, EditorRequired]
        public EventCallback GetMoreEvent { get; set; } = default!;        
        [Parameter]
        public EventCallback<(T, T)> OnReply { get; set; }
        [Parameter]
        public EventCallback<T> OnDelete { get; set; }
        [Parameter]
        public EventCallback<T> OnReacting { get; set; }

        private async Task GetMore()
        {
            await GetMoreEvent.InvokeAsync();
        }
    }
}
