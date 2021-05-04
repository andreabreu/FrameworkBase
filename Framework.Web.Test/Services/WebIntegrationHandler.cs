using Framework.Core.MessageBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Web.Test.Services
{
    public class WebIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        readonly ILogger _log = Log.ForContext<WebIntegrationHandler>();

        public WebIntegrationHandler(IMessageBus bus)
        {
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<Message>("msg", async (message, token) => await DoAction(message));
        }

        private async Task DoAction(Message message)
        {

            var text = JsonSerializer.Serialize<dynamic>(Encoding.UTF8.GetString(message.Body));

            _log.Information("DoAction: " + text);

            Console.WriteLine(text);
        }

    }
}
