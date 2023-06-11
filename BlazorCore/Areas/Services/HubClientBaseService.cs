using BlazorCore.Areas.Interfaces;
using DotNetCore.Entities.MessageAggregate;
using DotNetCore.Hubs;
using DotNetCore.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorCore.Areas.Services
{
    public abstract class HubClientBaseService : HubBaseService, IHubClientBaseService, IDisposable
    {     
        private readonly Dictionary<string, IDisposable> _subscriptions;
        public HubClientBaseService(NavigationManager navManager) : base(navManager)
        {
            _subscriptions = new Dictionary<string, IDisposable>();
        }

        public void Dispose()
        {
            foreach (var subscription in _subscriptions.Values)
            {
                subscription.Dispose();
            }
        }        

        public string SubscribeToMethod(Func<HubMessage, Task> handler, string? detailMethod = null)
        {
            var method = BaseHub.DetailedMethod(detailMethod);

            var subscription = HubConnection.On(
                    method, handler);         

            _subscriptions.Add(method, subscription);

            return method;
        }  
        
        public void UnsubscribeToMethod(string? detailMethod = null)
        {
            var method = BaseHub.DetailedMethod(detailMethod);

            if (_subscriptions.ContainsKey(method))
            {
                _subscriptions[method].Dispose();

                _subscriptions.Remove(method);
            }
        }
    }
}
