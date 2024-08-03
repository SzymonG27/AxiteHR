namespace AxiteHR.Services.AuthAPI.Models.Auth.Dto
{
	public record UserMessageBusDto
	{
		public string UserId { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string TempPassword { get; set; } = string.Empty;
	}
}
