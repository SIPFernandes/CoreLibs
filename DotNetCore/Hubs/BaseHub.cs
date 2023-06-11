using System.Text.Json.Serialization;
using DotNetCore.Entities.MessageAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DotNetCore.Hubs
{
    [Authorize]
    public class BaseHub : Hub
    {
        public const string HubUrl = "/basehub";

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        //Don't await because Connection Id might no longer be available and exception is thrown
        public Task LeaveRoom(string roomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task SendGroupMessage(BaseGroupMsgModel message, string? detailMethod = null)
        {            
            await Clients.OthersInGroup(message.RoomId)
                .SendAsync(DetailedMethod(detailMethod), message);
        }

        public async Task SendDirectMessage(BaseDirectMsgModel message, string? detailMethod = null)
        {
            await Clients.User(message.UserId).SendAsync(DetailedMethod(detailMethod), message);
        }

        public async Task SendMessage(HubMessage message, string? detailMethod = null)
        {
            await Clients.Others.SendAsync(DetailedMethod(detailMethod), message);
        }

        public static string DetailedMethod(string? detailMethod)
        {
            return detailMethod is null ? BaseHubConst.Communication.ReceiveMessage :
                BaseHubConst.Communication.ReceiveMessage + detailMethod;
        }

        public class BaseGroupMsgModel : HubMessage
        {
            public string RoomId { get; private set; }
            public BaseGroupMsgModel(string roomId, object obj,
                string type, string creatorId) : base(obj, type, creatorId)
            {
                RoomId = roomId;
            }
        }

        public class BaseDirectMsgModel : HubMessage
        {
            public string UserId { get; private set; }
            public BaseDirectMsgModel(string userId, object obj,
                string type, string creatorId) : base(obj, type, creatorId)
            {
                UserId = userId;
            }
        }        
    }

    public class BaseHubConst
    {
        public class Channels
        {
            public const string BaseFeed = "BaseFeed";            
        }

        public class Groups
        {
            public const string JoinRoom = nameof(BaseHub.JoinRoom);
            public const string LeaveRoom = nameof(BaseHub.LeaveRoom);
            public const string SendGroupMessage = nameof(BaseHub.SendGroupMessage);
        }

        public class Communication
        {
            public const string ReceiveMessage = "ReceiveMessage";            
            public const string SendDirectMessage = nameof(BaseHub.SendDirectMessage);
            public const string SendMessage = nameof(BaseHub.SendMessage);            
        }

        public class Separator
        {
            public const string Main = "|";
            public const string Secondary = ";";
            public const string Tertiary = ",";
        }

        public class MessageConfig
        {
            public const int CHUNKSIZE = 32768; //32kb
        }
    }
}
