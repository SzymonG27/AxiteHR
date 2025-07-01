namespace AxiteHR.Integration.Storage.Configuration
{
	public record MinioConfig
	{
		public string Endpoint { get; set; } = null!;
		public string AccessKey { get; set; } = null!;
		public string SecretKey { get; set; } = null!;
	}
}
