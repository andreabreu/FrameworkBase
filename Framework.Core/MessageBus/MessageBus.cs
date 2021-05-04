using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Serilog;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private ServiceBusClient _bus;

        private readonly IQueueClient _queueClient;

        private readonly string _connectionString;
        private readonly string _queueName;

        public MessageBus(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
            _queueClient = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
        }

        public async Task SendMessageAsync<T>(T obj) where T : class
        {
            await using (_bus = new ServiceBusClient(_connectionString))
            {
                ServiceBusSender sender = _bus.CreateSender(_queueName);

                string body = JsonSerializer.Serialize(obj);

                ServiceBusMessage message = new(body);

                await sender.SendMessageAsync(message);
            }
        }

        public void SubscribeAsync<T>(string subscriptionId, Func<Message, CancellationToken, Task> onMessage) where T : class
        {
            try
            {
                _queueClient.RegisterMessageHandler(onMessage, new MessageHandlerOptions(OnException));
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Error");
                Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
            }

        }

        static Task OnException(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine("Got an exception:");
            Console.WriteLine(args.Exception.Message);
            Console.WriteLine(args.ExceptionReceivedContext.Action);
            Console.WriteLine(args.ExceptionReceivedContext.ClientId);
            Console.WriteLine(args.ExceptionReceivedContext.Endpoint);
            Console.WriteLine(args.ExceptionReceivedContext.EntityPath);
            return Task.CompletedTask;
        }
    }
}
