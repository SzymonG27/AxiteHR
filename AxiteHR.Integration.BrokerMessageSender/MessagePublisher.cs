using AxiteHR.Integration.BrokerMessageSender.Models;
using AxiteHR.Integration.BrokerMessageSender.Senders.Factory;

namespace AxiteHR.Integration.BrokerMessageSender
{
	public class MessagePublisher(IBrokerMessageSenderFactory messageSenderFactory)
	{
		public virtual async Task PublishMessageAsync<TConfig, TMessage>(MessageSenderModel<TConfig, TMessage> model) where TConfig : IBrokerConfig
		{
			var sender = messageSenderFactory.GetSender<TConfig>();
			await sender.PublishMessageAsync(model);
		}
	}
}