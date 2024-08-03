using Microsoft.AspNetCore.Identity;

namespace AxiteHR.Services.AuthAPI.Models.Auth
{
	public class AppUser : IdentityUser
	{
		public virtual string FirstName { get; set; } = string.Empty;

		public virtual string LastName { get; set; } = string.Empty;

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member.
		//Modifyed column Email in the database to not nullable
		public override string Email { get; set; } = string.Empty;
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member.

		public virtual bool IsTempPassword { get; set; }
	}
}
