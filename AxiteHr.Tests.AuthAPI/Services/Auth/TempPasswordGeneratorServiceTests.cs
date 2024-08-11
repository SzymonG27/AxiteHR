using AxiteHR.Services.AuthAPI.Services.Auth.Impl;

namespace AxiteHr.Tests.AuthAPI.Services.Auth;

[TestFixture]
public class TempPasswordGeneratorServiceTests
{
	private TempPasswordGeneratorService _tempPasswordGeneratorService;

	[SetUp]
	public void SetUp()
	{
		_tempPasswordGeneratorService = new TempPasswordGeneratorService();
	}

	[Test]
	public void GenerateTempPassword_ShouldReturnPasswordOfCorrectLength()
	{
		// Act
		var tempPassword = _tempPasswordGeneratorService.GenerateTempPassword();

		// Assert
		Assert.That(tempPassword, Has.Length.EqualTo(12), "Generated password should be 12 characters long.");
	}

	[Test]
	public void GenerateTempPassword_ShouldContainAtLeastOneUppercaseLetter()
	{
		// Act
		var tempPassword = _tempPasswordGeneratorService.GenerateTempPassword();

		// Assert
		Assert.That(tempPassword, Does.Match("[A-Z]"), "Generated password should contain at least one uppercase letter.");
	}

	[Test]
	public void GenerateTempPassword_ShouldContainAtLeastOneLowercaseLetter()
	{
		// Act
		var tempPassword = _tempPasswordGeneratorService.GenerateTempPassword();

		// Assert
		Assert.That(tempPassword, Does.Match("[a-z]"), "Generated password should contain at least one lowercase letter.");
	}

	[Test]
	public void GenerateTempPassword_ShouldContainAtLeastOneDigit()
	{
		// Act
		var tempPassword = _tempPasswordGeneratorService.GenerateTempPassword();

		// Assert
		Assert.That(tempPassword, Does.Match(@"\d"), "Generated password should contain at least one digit.");
	}

	[Test]
	public void GenerateTempPassword_ShouldContainAtLeastOneSpecialCharacter()
	{
		// Act
		var tempPassword = _tempPasswordGeneratorService.GenerateTempPassword();

		// Assert
		Assert.That(tempPassword, Does.Match(@"[!@#$%^&*()_\-+=<>?]"), "Generated password should contain at least one special character.");
	}

	[Test]
	public void GenerateTempPassword_ShouldBeRandomOnEachCall()
	{
		// Act
		var tempPassword1 = _tempPasswordGeneratorService.GenerateTempPassword();
		var tempPassword2 = _tempPasswordGeneratorService.GenerateTempPassword();

		// Assert
		Assert.That(tempPassword1, Is.Not.EqualTo(tempPassword2), "Generated passwords should be different on each call.");
	}
}