using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Blobs;
using Newtonsoft.Json;

namespace SendingToEventHub
{
	class Program
	{

		static async Task Main(string[] args)
		{
			Console.WriteLine("Starting our Event Hub Receiver");
			string namespaceConnectionString = "Endpoint=sb://youtubeeventhubdemo.servicebus.windows.net/;SharedAccessKeyName=sendandreceive;SharedAccessKey=Lz65+NZ5N0zwZRkvih+8kXfuD0tYWV7Sk1nNdY+xA+0=;EntityPath=demoeventhub";
			string eventHubName = "demoeventhub";

			string blobConnectionString = "DefaultEndpointsProtocol=https;AccountName=eventhubstorageaccount3;AccountKey=ncQZvl23zgA6eS2+KUJ6IERd/emASeUFmBU2kVv/XYbepeA+h3HdyuGJ6g4WtFZ+gGOAfD6uP/tA9acXD29P8g==;EndpointSuffix=core.windows.net";
			string containerName = "offsetcontainer";
            BlobContainerClient blobContainerClient = new BlobContainerClient(blobConnectionString, containerName);
            EventProcessorClient processor = new EventProcessorClient(blobContainerClient, "$Default", namespaceConnectionString, eventHubName);

			processor.ProcessEventAsync += Processor_ProcessEventAsync;
			processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
			
			await processor.StartProcessingAsync();
			Console.WriteLine("Started the processor");

			Console.ReadLine();
			await processor.StopProcessingAsync();
			Console.WriteLine("Started the processor");
        }

		private static Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
		{
			Console.WriteLine("Error Received: " + arg.Exception.ToString());
			return Task.CompletedTask;
		}

		private static async Task Processor_ProcessEventAsync(ProcessEventArgs arg)
		{
			Console.WriteLine($"Event Received from Partition {arg.Partition.PartitionId}: {arg.Data.EventBody.ToString()}");
            await arg.UpdateCheckpointAsync();
        }
	}
}
