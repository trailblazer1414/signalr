using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SignalrClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection _connection;
        public MainWindow()
        {
            InitializeComponent();

            _connection = new HubConnectionBuilder()
                .WithUrl("http://127.0.0.1:52776/relayhub", options =>
                {
                    options.UseDefaultCredentials = true;
                    //options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets | Microsoft.AspNetCore.Http.Connections.HttpTransportType.ServerSentEvents;

                })
                .AddMessagePackProtocol()
                .Build();

            _connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await _connection.StartAsync();
            };
        }

        private async void connectButton_Click(object sender, RoutedEventArgs e)
        {
            _connection.On< string>("SendMessage", (message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    messagesList.Items.Add(message);
                });
            });

            try
            {
                await _connection.StartAsync();
                messagesList.Items.Add("Connection started");
                connectButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                messagesList.Items.Add(ex.Message);
            }
        }

    }
}

