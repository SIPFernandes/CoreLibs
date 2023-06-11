namespace BlazorCore.Data.Models.FeedModels
{
    public class FeedOptsModel : ReactionsOptions
    {
        public bool DisableComments { get; set; }
        public bool NestedReplies { get; set; }
        public bool CommentsSoftDelete { get; set; }
        public bool Realtime { get; set; }
        public bool SendNotifications { get; set; }
    }
}
