using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AxiteHR.Services.AuthAPI.Models.Auth
{
	public class AppUser : IdentityUser
	{
		[MaxLength(100)]
		public virtual string FirstName { get; set; } = string.Empty;
		[MaxLength(100)]
		public virtual string LastName { get; set; } = string.Empty;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member.
		//Modifyed column Email in the database to not nullable
		[MaxLength(100)]
		public override string Email { get; set; } = string.Empty;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member.

		public virtual bool IsTempPassword { get; set; }
	}
}
