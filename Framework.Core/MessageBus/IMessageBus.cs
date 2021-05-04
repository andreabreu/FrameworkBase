using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.MessageBus
{
    public interface IMessageBus
    {
        void SubscribeAsync<T>(string subscriptionId, Func<Message, CancellationToken, Task> onMessage) where T : class;
    }
}
