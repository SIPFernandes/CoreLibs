using DotNetCore.Interfaces;
using System.Linq.Expressions;

namespace DotNetCore.Services
{
    //TODO move to Blazor Core
    //TODO allow every GetMethod to receive a Select<T,J> being J the generic return type passed to the method and create PostView Model in BlazorShared with the specific properties for the frontend display
    public class GetItemsService<T> : IGetItemsService<T> where T : IAggregateRoot
    {
        protected IItemService<T> _service;
        public Expression<Func<T, bool>>? CurrentExpression { get; set; }
        public bool ItemsLoaded { get; private set; }
        private bool LoadingMoreItems;        

        public GetItemsService(IItemService<T> service)
        {
            _service = service;
        }

        public async Task<List<T>> GetNItems(Expression<Func<T, bool>>? expression = null,
            int take = 0, bool itemsLoaded = false)
        {
            ItemsLoaded = itemsLoaded;                      

            return await GetItems(expression, 0, take);
        }

        public async Task<List<T>> GetNOrderedItems<Z>(Expression<Func<T, Z>> orderedBy, bool descending = true,
            Expression<Func<T, bool>>? expression = null, int take = 0, bool itemsLoaded = false)
        {
            ItemsLoaded = itemsLoaded;

            return await GetItemsOrdered(orderedBy, descending, expression, 0, take);
        }

        public async Task<List<T>> GetRecentItems(Expression<Func<T, bool>>? expression)
        {
            ItemsLoaded = false;

            CurrentExpression = expression;

            return await GetItems(expression: CurrentExpression);
        }

        public async Task<List<W>> GetRecentItems<W>(Expression<Func<T, W>> selector,
            Expression<Func<T, bool>>? expression = null)
        {
            ItemsLoaded = false;            

            CurrentExpression = expression;

            return await GetItems(selector, CurrentExpression);
        }

        public async Task<List<T>> GetOrderedItems<Z>(Expression<Func<T, Z>> orderedBy, bool descending = true,
            Expression<Func<T, bool>>? expression = null)
        {
            ItemsLoaded = false;            

            CurrentExpression = expression;

            return await GetItemsOrdered<Z>(orderedBy, descending, CurrentExpression);
        }

        public async Task<List<W>> GetOrderedItems<W, Z>(Expression<Func<T, W>> selector,
            Expression<Func<T, Z>> orderedBy, Expression<Func<T, bool>>? expression = null)
        {
            ItemsLoaded = false;

            CurrentExpression = expression;

            return await GetItemsOrdered(selector, orderedBy, CurrentExpression);
        }

        public async Task GetMoreItems(List<T>? currentItems)
        {
            if (!ItemsLoaded && !LoadingMoreItems)
            {
                LoadingMoreItems = true;

                currentItems ??= new();

                var newItems = await GetItems(CurrentExpression, currentItems.Count);

                currentItems.AddRange(newItems);

                LoadingMoreItems = false;
            }
        }

        public async Task GetMoreOrderedItems<Z>(List<T>? currentItems,
            Expression<Func<T, Z>> orderedBy, bool descending = true)
        {
            if (!ItemsLoaded && !LoadingMoreItems)
            {
                LoadingMoreItems = true;

                currentItems ??= new();

                var newItems = await GetItemsOrdered(orderedBy, descending, CurrentExpression, currentItems.Count);

                currentItems.AddRange(newItems);

                LoadingMoreItems = false;
            }
        }

        public async Task GetMoreItems<W>(Expression<Func<T, W>> selector, List<W>? currentItems)
        {
            if (!ItemsLoaded && !LoadingMoreItems)
            {
                LoadingMoreItems = true;

                currentItems ??= new();

                var newItems = await GetItems(selector, CurrentExpression, currentItems.Count);

                currentItems.AddRange(newItems);

                LoadingMoreItems = false;
            }
        }

        private async Task<List<T>> GetItems(Expression<Func<T, bool>>? expression = null,
            int skip = 0, int take = 20)
        {
            var newItems = await _service.GetItems(expression, skip, take);

            if (newItems.Count < take)
            {
                ItemsLoaded = true;
            }

            return newItems;
        }

        private async Task<List<W>> GetItems<W>(Expression<Func<T, W>> selector, 
            Expression<Func<T, bool>>? expression = null, int skip = 0, int take = 20)
        {
            var newItems = await _service.GetItems(selector, expression, skip, take);

            if (newItems.Count < take)
            {
                ItemsLoaded = true;
            }

            return newItems;
        }

        private async Task<List<T>> GetItemsOrdered<Z>(Expression<Func<T, Z>> orderedBy,
            bool descending, Expression<Func<T, bool>>? expression = null, int skip = 0, int take = 20)
        {
            var newItems = await _service.GetItemsOrdered(orderedBy, descending, expression, skip, take);

            if (newItems.Count < take)
            {
                ItemsLoaded = true;
            }

            return newItems;
        }

        private async Task<List<W>> GetItemsOrdered<W, Z>(Expression<Func<T, W>> selector,
            Expression<Func<T, Z>> orderedBy, Expression<Func<T, bool>>? expression = null, 
            int skip = 0, int take = 20)
        {
            var newItems = await _service.GetItemsOrdered(selector, orderedBy, expression, skip, take);

            if (newItems.Count < take)
            {
                ItemsLoaded = true;
            }

            return newItems;
        }
    }
}
