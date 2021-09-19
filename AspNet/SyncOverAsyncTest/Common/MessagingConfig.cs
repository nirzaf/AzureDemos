

namespace Common
{
	public class MessagingConfig
	{
        public string ConnectionString { get; set; }
		public string RequestSubscription { get; set; }
		public string ReplySubscription { get; set; }
		public string RequestTopic { get; set; }
		public string ReplyTopic { get; set; }
    }
}
