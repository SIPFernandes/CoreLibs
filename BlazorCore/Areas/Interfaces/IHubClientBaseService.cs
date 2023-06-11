using DotNetCore.Entities.MessageAggregate;
using DotNetCore.Interfaces;

namespace BlazorCore.Areas.Interfaces
{
    public interface IHubClientBaseService : IHubBaseService
    {
        public string SubscribeToMethod(Func<HubMessage, Task> handler, string? detailMethod = null);
        public void UnsubscribeToMethod(string? detailMethod = null);
    }
}
