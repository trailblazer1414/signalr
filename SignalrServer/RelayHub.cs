using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace SignalrServer
{
    public interface IRelayHubClient
    {
        Task SendMessage(string messages);

    }

    [Authorize]
    public class RelayHub : Hub<IRelayHubClient>
    {
        private ILogger<RelayHub> _logger;

        public RelayHub(ILogger<RelayHub> logger)
        {
            _logger = logger;
        }

        private string UserId
        {
            get
            {
                return Context.User.Identity.Name;
            }
        }
        private string ConnectionId
        {
            get
            {
                return Context.ConnectionId;
            }
        }


        public override async Task OnConnectedAsync()
        {
            try
            {
                _logger.LogInformation("Connected : " + Context.ConnectionId + " User :" + UserId + " On :" + Context.Features.Get<IHttpTransportFeature>().TransportType);
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error connecting: " + ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    _logger.LogInformation("Error:" + exception);
                }

                _logger.LogInformation("Disconnected :" + Context.ConnectionId + " User :" + UserId + " On :" + Context.Features.Get<IHttpTransportFeature>().TransportType);
                await base.OnDisconnectedAsync(exception);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error disconnecting: " + ex.Message);
            }
        }

    }
    
}
