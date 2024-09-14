using AxiteHR.Integration.BrokerMessageSender.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AxiteHR.Integration.BrokerMessageSender.Senders.Factory
{
	public class BrokerMessageSenderFactory(IServiceProvider serviceProvider) : IBrokerMessageSenderFactory
	{
		public IBrokerMessageSender<TConfig> GetSender<TConfig>() where TConfig : IBrokerConfig
		{
			var sender = serviceProvider.GetService<IBrokerMessageSender<TConfig>>();
			return sender ?? throw new NotSupportedException($"There is no registered message sender for the configuration type {typeof(TConfig).Name}");
		}
	}
}
