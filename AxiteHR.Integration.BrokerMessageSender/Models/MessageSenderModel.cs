namespace AxiteHR.Integration.BrokerMessageSender.Models
{
	public record MessageSenderModel<TConfig>
		where TConfig : IBrokerConfig
	{
		public TConfig Config { get; set; } = default!;
		public object Message { get; set; } = default!;
	}
}
