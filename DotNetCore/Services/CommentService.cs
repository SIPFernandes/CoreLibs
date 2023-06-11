using DotNetCore.Entities.MessageAggregate.CommentAggregate;
using DotNetCore.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DotNetCore.Services
{
    public class CommentService<T> : ICommentService<T> where T : Comment
    {
        private readonly ICommentRepository<T> _commentRepository;
        private readonly ILogger<CommentService<T>> _logger;

        public CommentService(ICommentRepository<T> commentRepository,
            ILogger<CommentService<T>> logger)
        {
            _commentRepository = commentRepository;

            _logger = logger;
        }

        public async Task<List<T>> GetItems(Expression<Func<T, bool>>? expression = null,
            int skip = 0, int take = 20)
        {
            Expression<Func<T, T>>? selector = null;
            string[]? includes = null;

            if (expression != null && expression.Body.ToString().Contains(nameof(Comment.CommentReplied)))
            {
                var type = typeof(T);

                selector = x => (T) Activator.CreateInstance(type, x, true);

                includes = new string[] { nameof(Comment.Replies), nameof(Comment.Reactions) };
            }            

            return await _commentRepository.GetNItemsWhere(selector, expression, skip, take, includes);
        }

        public async Task<List<W>> GetItems<W>(Expression<Func<T, W>> selector, Expression<Func<T, bool>>? expression = null, int skip = 0, int take = 20)
        {
            return await _commentRepository.GetNItemsWhere(selector, expression, skip, take);            
        }

        public async Task<T> InsertComment(T comment)
        {                                    
            return await _commentRepository.Insert(comment);
        }

        public async Task DeleteComment(int commentId, bool softDelete, bool nestedReplies)
        {            
            if (softDelete)
            {
                await _commentRepository.UpdateMultipleLeafType(x => x.Id == commentId,
                    x => x.SetProperty(y => y.IsDeleted, true));
            }
            else if (nestedReplies)
            {
                await _commentRepository.DeleteMultipleLeafType(x => x.Id == commentId || x.ReplyToId == commentId);
            }
            else
            {                                
                await _commentRepository.UpdateMultipleLeafType(x => x.ReplyToId == commentId,
                    x => x.SetProperty(y => y.ReplyToId, y => null));

                await _commentRepository.DeleteById(commentId);
            }
        }

        public async Task UpdateComment(T comment, string text)
        {
            comment.Text = text;

            await _commentRepository.Update(comment);
        }

        public async Task<List<T>> GetItemsOrdered<Z>(Expression<Func<T, Z>> orderedBy,
            bool descending, Expression<Func<T, bool>>? expression = null,
            int skip = 0, int take = 20)
        {
            return await _commentRepository.GetNItemsWhereOrdered(orderedBy, descending, expression, skip, take);
        }

        public Task<List<W>> GetItemsOrdered<W, Z>(Expression<Func<T, W>> selector,
            Expression<Func<T, Z>> orderedBy, Expression<Func<T, bool>>? expression = null, int skip = 0, int take = 20)
        {
            throw new NotImplementedException();
        }
    }
}
