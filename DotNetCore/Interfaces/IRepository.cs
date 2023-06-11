using DotNetCore.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace DotNetCore.Interfaces
{
    public interface IRepository<T> where T : BaseEntity, IAggregateRoot
    {
        Task<List<T>> GetAll(CancellationTokenSource? token = null);
        Task<List<W>> GetNItemsWhere<W>(Expression<Func<T, W>> selector, 
            Expression<Func<T, bool>>? filter = null, int skip = 0, int take = 10,
            string[]? includes = null, CancellationTokenSource? token = null);
        Task<List<T>> GetNItemsWhereOrdered<Z>(Expression<Func<T, Z>> orderedBy, 
            bool descending, Expression<Func<T, bool>>? filter = null,
            int skip = 0, int take = 10, string[]? includes = null,
            CancellationTokenSource? token = null);
        Task<List<W>> GetNItemsWhereOrdered<W, Z>(Expression<Func<T, W>> selector,
            Expression<Func<T, Z>> orderedBy, Expression<Func<T, bool>>? filter = null,
            int skip = 0, int take = 10, string[]? includes = null, 
            CancellationTokenSource? token = null);
        Task<List<T>> GetNItemsWhere(Expression<Func<T, T>>? selector = null, 
            Expression<Func<T, bool>>? filter = null, int skip = 0, int take = 10,
            string[]? includes = null, CancellationTokenSource ? token = null);
        Task<T> Get(int id);
        Task<W> Get<W>(int id, Expression<Func<T, W>> selector) where W : BaseEntity;
        Task<T> Insert(T entity, CancellationTokenSource? token = null);
        Task Update(T entity, CancellationTokenSource? token = null);
        Task UpdateMultipleLeafType(Expression<Func<T, bool>> expression,
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyExpression,
            CancellationTokenSource? token = null);
        Task Delete(T entity, CancellationTokenSource? token = null);
        Task<T> DeleteById(int id, CancellationTokenSource? token = null);
        Task DeleteMultiple(IEnumerable<T> list, CancellationTokenSource? token = null);        
        Task DeleteMultipleLeafType(Expression<Func<T, bool>> expression,
            CancellationTokenSource? token = null);        
    }
}
