using System.Linq.Expressions;

namespace DotNetCore.Interfaces
{
    public interface IGetItemsService<T> where T : IAggregateRoot
    {
        public bool ItemsLoaded { get; }
        public Expression<Func<T, bool>>? CurrentExpression { get; set; }
        public Task<List<T>> GetNItems(Expression<Func<T, bool>>? expression = null,
            int take = 0, bool itemsLoaded = false);
        public Task<List<T>> GetNOrderedItems<Z>(Expression<Func<T, Z>> orderedBy, bool descending = true,
            Expression<Func<T, bool>>? expression = null, int take = 0, bool itemsLoaded = false);
        public Task<List<T>> GetRecentItems(Expression<Func<T, bool>>? expression = null);
        public Task<List<T>> GetOrderedItems<Z>(Expression<Func<T, Z>> orderedBy, bool descending = true,
            Expression<Func<T, bool>>? expression = null);
        public Task<List<W>> GetRecentItems<W>(Expression<Func<T, W>> selector, 
            Expression<Func<T, bool>>? expression = null);
        public Task<List<W>> GetOrderedItems<W, Z>(Expression<Func<T, W>> selector,
            Expression<Func<T, Z>> orderedBy, Expression<Func<T, bool>>? expression = null);
        public Task GetMoreItems(List<T>? currentItems);
        public Task GetMoreItems<W>(Expression<Func<T, W>> selector, List<W>? currentItems);
        public Task GetMoreOrderedItems<Z>(List<T>? currentItems,
            Expression<Func<T, Z>> orderedBy, bool descending = true);
    }
}
