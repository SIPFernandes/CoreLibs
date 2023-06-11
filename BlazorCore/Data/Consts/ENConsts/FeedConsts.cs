namespace BlazorCore.Data.Consts.ENConsts;
public class FeedConsts
{
    public const string Comment = "Comment";
    public const string AddComment = "Add a comment";
    public const string Commented = "commented:";
    public const string CommentEdited = " (edited)";
    public const string Bold = "B";
    public const string Reply = "Reply";
    public const string Replies = "Replies";
    public const string Author = "Author";
    public const string CommentDeleted = "This comment was deleted!";
    public const string Hide = "Hide";
    public const string Show = "Show";
    public const string Other = "Other";

    public class ConfirmModal
    {
        public const string DiscardChangesTitle = "Are you sure you want to discard this draft?"; 
        public const string DeleteCommentTitle = "Are you sure you want to delete this comment?"; 
        public const string DeleteCommentDescription = "Please note that if you choose to delete this item, all of its associated content and information will be permanently removed and cannot be retrieved."; 
    }

    public class EmptyStates
    {
        public const string CommentTitle = "Don’t be shy";
        public const string CommentDescription = "Feel free to share your opinions, questions, files, or ideas";
    }
}