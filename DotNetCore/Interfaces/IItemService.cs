using System.Linq.Expressions;

namespace DotNetCore.Interfaces
{
    public interface IItemService<T> where T : IAggregateRoot
    {        
        public Task<List<T>> GetItems(Expression<Func<T, bool>>? expression = null,
            int skip = 0, int take = 20);
        public Task<List<W>> GetItems<W>(Expression<Func<T, W>> selector, 
            Expression<Func<T, bool>>? expression = null, int skip = 0, int take = 20);
        public Task<List<T>> GetItemsOrdered<Z>(Expression<Func<T, Z>> orderedBy,
            bool descending, Expression<Func<T, bool>>? expression = null,
            int skip = 0, int take = 20);
        public Task<List<W>> GetItemsOrdered<W, Z>(Expression<Func<T, W>> selector,
            Expression<Func<T, Z>> orderedBy, Expression<Func<T, bool>>? expression = null,
            int skip = 0, int take = 20);
    }
}
