using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CompetingConsumersQueues
{
    class QueueReceiver
	{
		readonly string _queueName;
		readonly string _connectionString;
        readonly string _id;

		public QueueReceiver(string connectionString, string queueName, string id)
		{
			_connectionString = connectionString;
			_id = id;
			_queueName = queueName;
		}

		public void Go()
		{
			QueueClient receivingClient = new(_connectionString, _queueName, ReceiveMode.PeekLock, RetryPolicy.Default);
			receivingClient.PrefetchCount = 10;
			MessageHandlerOptions options = new(HandleError)
			{
				MaxConcurrentCalls = 1
			};
			receivingClient.RegisterMessageHandler(HandleMessage, options);
		}

		public Task HandleMessage(Message message, CancellationToken cancelToken)
		{
			var bodyBytes = message.Body;
			var ourMessage = System.Text.Encoding.UTF8.GetString(bodyBytes);
			Console.WriteLine($"Message Received on {_id}: {ourMessage}");
			return Task.CompletedTask;
		}

		public Task HandleError(ExceptionReceivedEventArgs args)
		{
			Console.WriteLine($"Exception Occurred on {_id}! : {args.Exception}");
			return Task.CompletedTask;
		}
	}
}
