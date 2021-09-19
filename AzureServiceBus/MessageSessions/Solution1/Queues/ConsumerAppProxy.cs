using System;
using System.Diagnostics;

namespace CompetingConsumersQueues
{
    class ConsumerAppProxy
	{
		public static IDisposable StartConsumerApp(string connectionString, string queueName, object id)
		{
			ProcessStartInfo psi = new ProcessStartInfo("ConsumerApp.exe", string.Join(' ', connectionString, queueName, id.ToString()))
			{
				CreateNoWindow = false,
				WindowStyle = ProcessWindowStyle.Normal,
				UseShellExecute = true,
			};
			var process = Process.Start(psi);	
			return new ProcessKiller(process);
		}

		class ProcessKiller : IDisposable
		{
			private Process p;
			public ProcessKiller(Process p) { this.p = p; }
			public void Dispose()
			{
				p.Kill(true);
			}
		}

		public static void IgnoreThisMethod()
		{
            _ = typeof(ConsumerApp.Program);
        }
	}
}
