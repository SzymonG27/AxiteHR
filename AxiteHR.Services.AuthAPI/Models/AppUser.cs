using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AxiteHR.Services.AuthAPI.Models
{
	public class AppUser : IdentityUser
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public override string Email { get; set; } = string.Empty;
	}
}
