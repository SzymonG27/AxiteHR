using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.AuthAPI.Models.Dto
{
	public class RegisterRequestDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[MinLength(5)]
		public string UserName { get; set; } = string.Empty;

		[Required]
		[MinLength(2)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[MinLength(2)]
		public string LastName { get; set; } = string.Empty;

		[Required]
		[MinLength(8)]
		public string UserPassword { get; set; } = string.Empty;

		[Required]
		[Compare(nameof(UserPassword))]
		public string UserPasswordRepeated { get; set; } = string.Empty;

		[Required]
		public string PhoneNumber { get; set; } = string.Empty;
	}
}
