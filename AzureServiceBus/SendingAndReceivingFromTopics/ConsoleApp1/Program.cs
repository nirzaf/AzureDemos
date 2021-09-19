using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Identity.Client;

namespace ConsoleApp1
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Starting Azure Service Bus Demo");

			string endpoint = $"sb://iaransyoutubedemos.servicebus.windows.net/";

			var connectionString =
				"<connection string>";

			var topicName = "demotopic";

			var tokenProvider = TokenProvider.CreateAzureActiveDirectoryTokenProvider(async (audience, authority, state) =>
			{
				var app = PublicClientApplicationBuilder.Create(ClientId)
							.WithRedirectUri(ConfigurationManager.AppSettings["redirectURI"])
							.Build();

				var serviceBusAudience = new Uri("https://servicebus.azure.net");

				var authResult = await app.AcquireTokenInteractive(new string[] { $"{serviceBusAudience}/.default" }).ExecuteAsync();

				return authResult.AccessToken;
			};

			TopicClient topicClient = new TopicClient(endpoint, topicName, 
);

			SubscriptionClient subscriptionClient = new SubscriptionClient(connectionString, topicName,
				"demosubscription1", ReceiveMode.PeekLock, RetryPolicy.Default);
			
			subscriptionClient.RegisterMessageHandler((message, cancelToken) =>
				{
					var bodyBytes = message.Body;
                    var ourMessage = System.Text.Encoding.UTF8.GetString(bodyBytes);
                    Console.WriteLine("Message Received:" + ourMessage);
                    return Task.CompletedTask;
				},
				(exceptionArg) =>
				{
                    Console.WriteLine("Exception Occurred! : " + exceptionArg.Exception.ToString());
                    return Task.CompletedTask;
				});


			while (true)
			{
				var messageText = Console.ReadLine();

				if (messageText == "quit") break;
				
				var ourMessage = System.Text.Encoding.UTF8.GetBytes(messageText);

				await topicClient.SendAsync(new Message(ourMessage));

			}
		}
	}
}