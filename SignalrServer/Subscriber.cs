using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignalrServer
{
    public class Subscriber : IHostedService
    {
        private readonly ILogger<Subscriber> _logger;
        private readonly IConfigurationRoot _config;
        private IHubContext<RelayHub, IRelayHubClient> _hubContext;

        public Subscriber(ILogger<Subscriber> logger, IConfigurationRoot config, IHubContext<RelayHub, IRelayHubClient> hubContext)
        {
            _logger = logger;
            _config = config;
            _hubContext = hubContext;
        }

        private void MakeConnection()
        {
            string message = "Quick brown fox jumped over the lazy dog";

            Observable.Interval(TimeSpan.FromMilliseconds(10)).Subscribe((y) =>
            {
                _hubContext.Clients.All.SendMessage(message);
            });
        }

       
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(()=> { MakeConnection(); });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

       
  
    }

}
