using Common;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SyncOverAsyncTest
{
	public class CommunicationService : ICommunicationService
	{
		private readonly MessagingConfig _config;
        private readonly TopicClient _topicClient;
        private readonly SubscriptionClient _subscriptionClient;

		public CommunicationService(MessagingConfig config)
		{
			_config = config;
            _topicClient = new TopicClient(config.ConnectionString, config.RequestTopic);
            _subscriptionClient = new SubscriptionClient(config.ConnectionString, config.ReplyTopic, config.ReplySubscription);
            _subscriptionClient.RegisterMessageHandler(HandleMessage, HandleError);
        }


        private static Task HandleError(ExceptionReceivedEventArgs args)
		{
			return Task.CompletedTask;
		}


        readonly Dictionary<Guid, StateObj> _states = new();

        private Task HandleMessage(Message message, CancellationToken cancelToken)
		{
            var response = JsonConvert.DeserializeObject<ResponseMessage>(System.Text.Encoding.UTF8.GetString(message.Body));
            if (!_states.TryGetValue(response.Id, out var state)) return Task.CompletedTask;
            state.TaskSource.SetResult(new GetNumberResult { Id = response.Id, Number = response.Number });
            _states.Remove(response.Id);
            return Task.CompletedTask;
		}


		public async Task<GetNumberResult> DoAsyncWork(Guid Id)
		{
            var state = new StateObj { Id = Id, TaskSource = new TaskCompletionSource<GetNumberResult>() };
            _states.Add(Id, state);
            await _topicClient.SendAsync(new Message(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new RequestMessage { Id = Id }))));
            return await state.TaskSource.Task;
        }

        private class StateObj
		{
			public Guid Id;
			public TaskCompletionSource<GetNumberResult> TaskSource;
		}
	}
}