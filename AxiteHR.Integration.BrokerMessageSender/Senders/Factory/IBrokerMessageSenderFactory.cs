using AxiteHR.Integration.BrokerMessageSender.Models;

namespace AxiteHR.Integration.BrokerMessageSender.Senders.Factory
{
	public interface IBrokerMessageSenderFactory
	{
		IBrokerMessageSender<TConfig> GetSender<TConfig>() where TConfig : IBrokerConfig;
	}
}
