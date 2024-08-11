using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AxiteHR.Services.AuthAPI.Models.Auth;
using AxiteHR.Services.AuthAPI.Services.Auth.Impl;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace AxiteHr.Tests.AuthAPI.Services.Auth;

[TestFixture]
public class JwtTokenGeneratorTests
{
	private JwtTokenGenerator _jwtTokenGenerator;
	private Mock<IOptions<JwtOptions>> _jwtOptionsMock;

	[SetUp]
	public void SetUp()
	{
		_jwtOptionsMock = new Mock<IOptions<JwtOptions>>();

		var jwtOptions = new JwtOptions
		{
			Secret = "my_super_secret_key_12345_12345_12345",
			Audience = "my_audience",
			Issuer = "my_issuer",
			ExpiresInMins = 30
		};

		_jwtOptionsMock.Setup(x => x.Value).Returns(jwtOptions);

		_jwtTokenGenerator = new JwtTokenGenerator(_jwtOptionsMock.Object);
	}

	[Test]
	public void GenerateToken_ShouldReturnValidJwtToken()
	{
		// Arrange
		var appUser = new AppUser
		{
			Id = "user1",
			Email = "user@example.com",
			FirstName = "John",
			LastName = "Doe",
			PhoneNumber = "1234567890"
		};

		var roleList = new List<string> { "Admin", "User" };

		// Act
		var token = _jwtTokenGenerator.GenerateToken(appUser, roleList);

		// Assert
		Assert.That(token, Is.Not.Null, "Token should not be null");

		var tokenHandler = new JwtSecurityTokenHandler();
		var secret = Encoding.ASCII.GetBytes(_jwtOptionsMock.Object.Value.Secret);
		var validationParameters = new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(secret),
			ValidateIssuer = true,
			ValidIssuer = _jwtOptionsMock.Object.Value.Issuer,
			ValidateAudience = true,
			ValidAudience = _jwtOptionsMock.Object.Value.Audience,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.Zero // Disable delay when validating token expiration
		};

		var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
		var jwtToken = validatedToken as JwtSecurityToken;

		Assert.Multiple(() =>
		{
			Assert.That(jwtToken, Is.Not.Null, "Validated token should not be null");

			Assert.That(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, Is.EqualTo(appUser.Id), "Sub claim should match user ID");
			Assert.That(principal.FindFirst(ClaimTypes.Email)?.Value, Is.EqualTo(appUser.Email), "Email claim should match user email");
			Assert.That(principal.FindFirst(ClaimTypes.GivenName)?.Value, Is.EqualTo(appUser.FirstName), "GivenName claim should match user first name");
			Assert.That(principal.FindFirst(ClaimTypes.Surname)?.Value, Is.EqualTo(appUser.LastName), "FamilyName claim should match user last name");
			Assert.That(principal.FindFirst("PhoneNumber")?.Value, Is.EqualTo(appUser.PhoneNumber), "PhoneNumber claim should match user phone number");
		});

		var roles = principal.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToList();
		CollectionAssert.AreEquivalent(roleList, roles, "Roles in token should match the provided roles");

		Assert.Multiple(() =>
		{
			Assert.That(jwtToken.Issuer, Is.EqualTo(_jwtOptionsMock.Object.Value.Issuer), "Issuer should match");
			Assert.That(jwtToken.Audiences.First(), Is.EqualTo(_jwtOptionsMock.Object.Value.Audience), "Audience should match");
		});

		var expectedExpiration = DateTime.UtcNow.AddMinutes(_jwtOptionsMock.Object.Value.ExpiresInMins);
		Assert.That(jwtToken.ValidTo, Is.EqualTo(expectedExpiration).Within(1).Minutes, "Token expiration time should be correct");
	}
}