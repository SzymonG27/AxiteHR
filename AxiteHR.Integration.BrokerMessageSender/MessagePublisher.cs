using AxiteHR.Integration.BrokerMessageSender.Models;
using AxiteHR.Integration.BrokerMessageSender.Senders.Factory;

namespace AxiteHR.Integration.BrokerMessageSender
{
	public class MessagePublisher(IBrokerMessageSenderFactory messageSenderFactory)
	{
		public async Task PublishMessageAsync<TConfig>(MessageSenderModel<TConfig> model) where TConfig : IBrokerConfig
		{
			var sender = messageSenderFactory.GetSender<TConfig>();
			await sender.PublishMessageAsync(model);
		}
	}
}