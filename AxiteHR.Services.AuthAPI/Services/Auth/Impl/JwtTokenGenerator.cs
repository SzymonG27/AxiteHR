﻿using AxiteHR.Services.AuthAPI.Models.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AxiteHR.Services.AuthAPI.Services.Auth.Impl
{
	public class JwtTokenGenerator(IOptions<JwtOptions> jwtOptions) : IJwtTokenGenerator
	{
		private readonly JwtOptions _jwtOptions = jwtOptions.Value;

		public string GenerateToken(AppUser appUser, IList<string> roleList)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var secret = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
			var claimList = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub, appUser.Id),
				new(JwtRegisteredClaimNames.Email, appUser.Email),
				new(JwtRegisteredClaimNames.GivenName, appUser.FirstName),
				new(JwtRegisteredClaimNames.FamilyName, appUser.LastName),
				new("PhoneNumber", appUser.PhoneNumber ?? ""),
			};

			claimList.AddRange(roleList.Select(role => new Claim(ClaimTypes.Role, role)));

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Audience = _jwtOptions.Audience,
				Issuer = _jwtOptions.Issuer,
				Subject = new ClaimsIdentity(claimList),
				Expires = DateTime.Now.AddMinutes(_jwtOptions.ExpiresInMins),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
