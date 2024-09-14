using AxiteHR.Integration.BrokerMessageSender.Models;

namespace AxiteHR.Integration.BrokerMessageSender.Senders
{
	public interface IBrokerMessageSender<TConfig>
		where TConfig : IBrokerConfig
	{
		Task PublishMessageAsync<TMessage>(MessageSenderModel<TConfig, TMessage> serviceBusSenderModel);
	}
}
