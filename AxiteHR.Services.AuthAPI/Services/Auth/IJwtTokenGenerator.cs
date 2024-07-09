using AxiteHR.Services.AuthAPI.Models.Auth;

namespace AxiteHR.Services.AuthAPI.Services.Auth
{
	public interface IJwtTokenGenerator
	{
		string GenerateToken(AppUser appUser, IList<string> roleList);
	}
}
