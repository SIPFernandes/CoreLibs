using DotNetCore.Entities.MessageAggregate;
using DotNetCore.Hubs;
using DotNetCore.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;
using static DotNetCore.Hubs.BaseHub;

namespace DotNetCore.Services
{
    public class HubBaseService : IHubBaseService, IAsyncDisposable
    {
        public HubConnection HubConnection { get; set; } = default!;
        private readonly NavigationManager _navManager;

        public HubBaseService(NavigationManager navManager) 
        {
            _navManager = navManager;
        }

        public async ValueTask DisposeAsync()
        {
            if (HubConnection != null)
            {
                await HubConnection.DisposeAsync();
            }             
        }

        public async Task StartHubConnection(Dictionary<string, string> Cookies)
        {
            if (HubConnection == null)
            {
                HubConnection = new HubConnectionBuilder()
                .WithUrl(_navManager.ToAbsoluteUri(HubUrl), options =>
                {
                    options.UseDefaultCredentials = true;
                    var cookieCount = Cookies.Count();
                    var cookieContainer = new CookieContainer(cookieCount);
                    foreach (var cookie in Cookies)
                        cookieContainer.Add(new Cookie(
                            cookie.Key,
                            WebUtility.UrlEncode(cookie.Value),
                            path: "/",
                            domain: _navManager.ToAbsoluteUri("/").Host));
                    options.Cookies = cookieContainer;

                    foreach (var header in Cookies)
                        options.Headers.Add(header.Key, header.Value);

                    options.HttpMessageHandlerFactory = (input) =>
                    {
                        var clientHandler = new HttpClientHandler
                        {
                            PreAuthenticate = true,
                            CookieContainer = cookieContainer,
                            UseCookies = true,
                            UseDefaultCredentials = true,
                        };
                        return clientHandler;
                    };
                })
                .WithAutomaticReconnect()                
                .Build();

                                   
                await HubConnection.StartAsync();                
            }
            else
            {
                throw new InvalidOperationException("Hub connection has already started");
            }
        }

        public async Task ConnectToGroup(string roomId)
        {           
            await HubConnection.SendAsync(BaseHubConst.Groups.JoinRoom,
                roomId);
        }

        public async Task LeaveGroup(string roomId)
        {
            await HubConnection.SendAsync(BaseHubConst.Groups.LeaveRoom,
                roomId);
        }

        public async Task SendGroupMessage(BaseGroupMsgModel message, string? detailMethod = null)
        {            
            await HubConnection.SendAsync(BaseHubConst.Groups.SendGroupMessage, message, detailMethod);
        }

        public async Task SendDirectMessage(BaseDirectMsgModel message, string? detailMethod = null)
        {            
            await HubConnection.SendAsync(BaseHubConst.Communication.SendDirectMessage, message, detailMethod);
        }

        public async Task SendMessage(HubMessage message, string? detailMethod = null)
        {         
            await HubConnection.SendAsync(BaseHubConst.Communication.SendMessage, message, detailMethod);
        }        
    }
}
