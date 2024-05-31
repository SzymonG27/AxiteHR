namespace AxiteHR.Services.AuthAPI.Models.Dto
{
	public class RegisterResponseDto
	{
		public bool IsRegisteredSuccessful { get; set; }
		public IList<string> ErrorMessage { get; set; } = new List<string>();
	}
}
