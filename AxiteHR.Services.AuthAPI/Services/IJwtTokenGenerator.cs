using AxiteHR.Services.AuthAPI.Models;

namespace AxiteHR.Services.AuthAPI.Services
{
	public interface IJwtTokenGenerator
	{
		string GenerateToken(AppUser appUser, IList<string> roleList);
	}
}
