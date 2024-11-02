using System.IdentityModel.Tokens.Jwt;

namespace AxiteHR.Integration.JwtTokenHandler
{
	public class JwtDecode : IJwtDecode
	{
		public Guid? GetUserIdFromToken(string token)
		{
			var jwtSecurityToken = GetJwtSecurityToken(token);
			if (jwtSecurityToken == null)
			{
				return null;
			}

			return Guid.Parse((string)jwtSecurityToken.Payload.Single(x => x.Key == JwtRegisteredClaimNames.Sub).Value);
		}

		private static JwtSecurityToken? GetJwtSecurityToken(string token)
		{
			var handler = new JwtSecurityTokenHandler();
			return handler.ReadJwtToken(token);
		}
	}
}
