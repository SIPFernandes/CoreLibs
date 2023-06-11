using BlazorCore.Data.Consts;
using Microsoft.AspNetCore.Components;

namespace BlazorCore.Components.CommentFeed
{
    public partial class CommentText : ComponentBase
    {
        [Parameter, EditorRequired]
        public string Text { get; set; } = default!;
        [Parameter, EditorRequired]
        public int CommentId { get; set; } = default!;
        private string[] MessageArray { get; set; } = default!;
        private string _commentText { get; set; } = default!;
        private int CHARLIMIT = 200;
        private int CharCount;
        private bool ShowMore;


        protected override void OnParametersSet()
        {
            if(_commentText != Text)
            {
                _commentText = Text;

                MessageArray = Text.Split(HtmlConsts.Space);
            }
        }
    }
}