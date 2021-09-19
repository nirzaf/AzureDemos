using System;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;

namespace SendingToEventHub
{
    class Program
    {
        static int _messageNumber = 0;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting our Event Hub Producer");
            string namespaceConnectionString = "Endpoint=sb://eventhubyoutubedemos2.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=KoaJlFSB5wTdAnwoHKrQA+fl2eoWHpjyBK9vkuo8aFw=";
            string eventHubName = "demoeventhub";

            await SendAFewMessages(namespaceConnectionString, eventHubName);
            Console.WriteLine("Messages Sent");
            Console.ReadLine();
        }

        private static async Task SendAFewMessages(string namespaceConnectionString, string eventHubName)
        {
            var client = new EventHubProducerClient(namespaceConnectionString, eventHubName);

            //var batch = await client.CreateBatchAsync(new CreateBatchOptions { PartitionKey = "hello" });
            EventData[] messages = new EventData[1];
            for (var x = 0; x < 10; x++)
            {
                var greetingPart = (x % 2 == 0 ? "Hello " : "World! ");
                var msgText = $"{ _messageNumber++} - {greetingPart}";
                var msg = new EventData(msgText);
                messages[0] = msg;
                await client.SendAsync(messages, new SendEventOptions { PartitionId = (x%2).ToString()});
            }
        }
    }
}
