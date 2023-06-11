using DotNetCore.Entities.MessageAggregate;
using static DotNetCore.Hubs.BaseHub;

namespace DotNetCore.Interfaces
{
    public interface IHubBaseService
    {          
        public Task StartHubConnection(Dictionary<string, string> Cookies);
        public Task ConnectToGroup(string roomId);
        public Task LeaveGroup(string roomId);
        public Task SendGroupMessage(BaseGroupMsgModel message, string? detailMethod = null);
        public Task SendDirectMessage(BaseDirectMsgModel message, string? detailMethod = null);
        public Task SendMessage(HubMessage message, string? detailMethod = null);
    }
}
