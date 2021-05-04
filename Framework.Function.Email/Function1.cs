using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Framework.Function.Email
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([ServiceBusTrigger("email", Connection = "MessageBus")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
