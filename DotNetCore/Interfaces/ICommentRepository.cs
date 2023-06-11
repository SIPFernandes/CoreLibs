using DotNetCore.Entities.MessageAggregate.CommentAggregate;

namespace DotNetCore.Interfaces
{
    public interface ICommentRepository<T> : IRepository<T> where T : Comment
    {        
    }
}
