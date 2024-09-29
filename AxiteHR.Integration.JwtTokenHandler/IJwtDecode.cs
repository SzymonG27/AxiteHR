namespace AxiteHR.Integration.JwtTokenHandler
{
	public interface IJwtDecode
	{
		Guid? GetUserIdFromToken(string token);
	}
}
