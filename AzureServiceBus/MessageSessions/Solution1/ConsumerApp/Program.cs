using CompetingConsumersQueues;
using System;
using System.Threading.Tasks;

namespace ConsumerApp
{
    public class Program
	{
		static async Task Main(string[] args)
		{
			var connectionString = args[0];	
			var queueName = args[1];
			var id = args.Length > 2 ? args[2] : "";
			QueueReceiver receiver1 = new(connectionString, queueName, id);
			receiver1.Go();				
			Console.ReadLine();
		}
	}
}
