using Framework.Core.MessageBus;
using Framework.Core.Utils;
using Framework.Web.Test.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Web.Test.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));

            services.AddHostedService<WebIntegrationHandler>();

        }
    }
}
