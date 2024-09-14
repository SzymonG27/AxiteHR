namespace AxiteHR.Integration.BrokerMessageSender.Models
{
	public record MessageSenderModel<TConfig, TMessage>
		where TConfig : IBrokerConfig
	{
		public TConfig Config { get; set; } = default!;
		public TMessage Message { get; set; } = default!;
	}
}
