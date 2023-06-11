namespace BlazorCore.Data.Consts;

public class CommentsConsts
{
    public const string UserItemClass = "user-item";

    public const string TagSection = "section";
    public const string TagValue = "content";
    public const string TagPattern = @"(?<=)<(?'" + TagSection + @"'\w+)>(?'" + TagValue + @"'.*?)<\/\1>";

    public static class Reactions
    {
        public const string ThumbsUp = "👍";
        public const string ThumbsDown = "👎";
        public const string Smile = "🙂";
        public const string Laugh = "😁";
        public const string Sad = "🙁";
    }

    public enum CommentTextTags
    {
        UserId,
        FileReference
    }
}