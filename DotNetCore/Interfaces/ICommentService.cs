using DotNetCore.Entities.MessageAggregate.CommentAggregate;

namespace DotNetCore.Interfaces
{
    public interface ICommentService<T> : IItemService<T> where T : Comment
    {
        public Task<T> InsertComment(T comment);
        public Task DeleteComment(int commentId, bool softDelete, bool nestedReplies = false);
        public Task UpdateComment(T comment, string text);        
    }
}
