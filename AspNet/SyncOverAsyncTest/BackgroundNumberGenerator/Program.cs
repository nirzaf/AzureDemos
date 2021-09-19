
using Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundNumberGenerator
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var s = new MessagingService(new MessagingConfig
			{
				ConnectionString = "Endpoint=sb://ciaransyoutubedemos.servicebus.windows.net/;SharedAccessKeyName=SendDemo;SharedAccessKey=YTx4cOCZrec/H+kalXfwuW6H4jkBpjPMfUAk9LrIC0c=",
				ReplyTopic = "replyTopic",
				RequestTopic = "requestTopic",
				RequestSubscription = "requestSubscription"
			});
            Console.CancelKeyPress += Console_CancelKeyPress;
            await Task.Delay(Convert.ToInt32(TimeSpan.FromHours(1).TotalMilliseconds), Cancel.Token);
        }

		static readonly CancellationTokenSource Cancel = new();
		private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
		{
			Cancel.Cancel();
		}
	}
}
