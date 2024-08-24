using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Integration.MessageBus;
using AxiteHR.Security.Encryption;
using AxiteHR.Services.AuthAPI.Data;
using AxiteHR.Services.AuthAPI.Helpers;
using AxiteHR.Services.AuthAPI.Models.Auth;
using AxiteHR.Services.AuthAPI.Models.Auth.Dto;
using AxiteHR.Services.AuthAPI.Models.EmployeeModels.Dto;
using AxiteHR.Services.AuthAPI.Services.Auth;
using AxiteHR.Services.AuthAPI.Services.Auth.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace AxiteHr.Tests.AuthAPI.Services.Auth;

[TestFixture]
public class AuthServiceTests
{
	private Mock<AppDbContext> _dbContextMock;
	private Mock<UserManager<AppUser>> _userManagerMock;
	private Mock<RoleManager<IdentityRole>> _roleManagerMock;
	private Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
	private Mock<IStringLocalizer<AuthResources>> _authLocalizerMock;
	private Mock<IMessageBus> _messageBusMock;
	private IConfiguration _configuration;
	private Mock<ITempPasswordGeneratorService> _tempPasswordGeneratorServiceMock;
	private Mock<IEncryptionService> _encryptionServiceMock;
	private AuthService _authService;

	[SetUp]
	public void SetUp()
	{
		_dbContextMock = MockAppDbContext();
		_userManagerMock = MockUserManager();
		_roleManagerMock = MockRoleManager();
		_jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
		_authLocalizerMock = new Mock<IStringLocalizer<AuthResources>>();
		_messageBusMock = new Mock<IMessageBus>();
		_configuration = MockConfiguration();
		_tempPasswordGeneratorServiceMock = new Mock<ITempPasswordGeneratorService>();
		_encryptionServiceMock = new Mock<IEncryptionService>();

		_authService = new AuthService(
			_dbContextMock.Object,
			_userManagerMock.Object,
			_roleManagerMock.Object,
			_jwtTokenGeneratorMock.Object,
			_authLocalizerMock.Object,
			_messageBusMock.Object,
			_configuration,
			_tempPasswordGeneratorServiceMock.Object,
			_encryptionServiceMock.Object
		);
	}

	[Test]
	public async Task LoginAsync_ValidCredentials_ReturnsSuccessLoginResponse()
	{
		// Arrange
		var user = new AppUser { Email = "test@example.com" };
		var loginRequest = new LoginRequestDto { Email = "test@example.com", Password = "password" };
		var roles = new List<string> { "User" };
		const string token = "generated-jwt-token";

		_userManagerMock.Setup(x => x.FindByEmailAsync(loginRequest.Email))
			.ReturnsAsync(user);

		_userManagerMock.Setup(x => x.CheckPasswordAsync(user, loginRequest.Password))
			.ReturnsAsync(true);

		_userManagerMock.Setup(x => x.GetRolesAsync(user))
			.ReturnsAsync(roles);

		_jwtTokenGeneratorMock.Setup(x => x.GenerateToken(user, roles))
			.Returns(token);

		_roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
			.ReturnsAsync(true);

		// Act
		var result = await _authService.LoginAsync(loginRequest);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsLoggedSuccessful, Is.True);
			Assert.That(result.Token, Is.EqualTo(token));
		});
	}

	[Test]
	public async Task RegisterAsync_UserAlreadyExists_ReturnsFailedResponse()
	{
		// Arrange
		var registerRequest = new RegisterRequestDto
		{
			Email = "test@example.com",
			UserName = "testuser",
			FirstName = "Test",
			LastName = "User",
			PhoneNumber = "1234567890",
			UserPassword = "password123"
		};

		_userManagerMock.Setup(x => x.FindByEmailAsync(registerRequest.Email))
			.ReturnsAsync(new AppUser { Email = registerRequest.Email });

		_authLocalizerMock.Setup(x => x[AuthResourcesKeys.RegisterEmailExistsInDb])
			.Returns(new LocalizedString(AuthResourcesKeys.RegisterEmailExistsInDb, "Email already exists"));

		// Act
		var result = await _authService.RegisterAsync(registerRequest);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsRegisteredSuccessful, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("Email already exists"));
		});
	}

	[Test]
	public async Task RegisterAsync_UserCreationFails_ReturnsFailedResponse()
	{
		// Arrange
		var registerRequest = new RegisterRequestDto
		{
			Email = "test@example.com",
			UserName = "testuser",
			FirstName = "Test",
			LastName = "User",
			PhoneNumber = "1234567890",
			UserPassword = "password123"
		};

		_userManagerMock.Setup(x => x.FindByEmailAsync(registerRequest.Email))
			.ReturnsAsync((AppUser?)null);

		_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerRequest.UserPassword))
			.ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to create user" }));

		_authLocalizerMock.Setup(x => x[AuthResourcesKeys.RegisterGlobalError])
			.Returns(new LocalizedString(AuthResourcesKeys.RegisterGlobalError, "User registration failed"));

		// Act
		var result = await _authService.RegisterAsync(registerRequest);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsRegisteredSuccessful, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("User registration failed"));
		});
	}

	[Test]
	public async Task RegisterAsync_SuccessfulRegistration_ReturnsSuccessResponse()
	{
		// Arrange
		var registerRequest = new RegisterRequestDto
		{
			Email = "test@example.com",
			UserName = "testuser",
			FirstName = "Test",
			LastName = "User",
			PhoneNumber = "1234567890",
			UserPassword = "password123"
		};

		_userManagerMock.Setup(x => x.FindByEmailAsync(registerRequest.Email))
			.ReturnsAsync((AppUser?)null);

		_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerRequest.UserPassword))
			.ReturnsAsync(IdentityResult.Success);

		_roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
		.ReturnsAsync(true); // Rola istnieje

		_userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
			.ReturnsAsync(IdentityResult.Success);

		_userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
			.ReturnsAsync([]);

		_dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		// Act
		var result = await _authService.RegisterAsync(registerRequest);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsRegisteredSuccessful, Is.True);
			Assert.That(result.ErrorMessage, Is.Empty);
			Assert.That(result.UserId, Is.Not.Empty);
		});
	}

	[Test]
	public async Task RegisterAsync_RoleDoesNotExist_ReturnsFailedResponse()
	{
		// Arrange
		var registerRequest = new RegisterRequestDto
		{
			Email = "test@example.com",
			UserName = "testuser",
			FirstName = "Test",
			LastName = "User",
			PhoneNumber = "1234567890",
			UserPassword = "password123"
		};

		_userManagerMock.Setup(x => x.FindByEmailAsync(registerRequest.Email))
			.ReturnsAsync((AppUser?)null);

		_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerRequest.UserPassword))
			.ReturnsAsync(IdentityResult.Success);

		_roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
			.ReturnsAsync(false); // Rola nie istnieje

		_authLocalizerMock.Setup(x => x[AuthResourcesKeys.RegisterGlobalError])
			.Returns(new LocalizedString(AuthResourcesKeys.RegisterGlobalError, "User registration failed"));

		// Act
		var result = await _authService.RegisterAsync(registerRequest);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsRegisteredSuccessful, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("User registration failed"));
		});
	}

	[Test]
	public async Task RegisterAsync_FailedToAddRole_ReturnsFailedResponse()
	{
		// Arrange
		var registerRequest = new RegisterRequestDto
		{
			Email = "test@example.com",
			UserName = "testuser",
			FirstName = "Test",
			LastName = "User",
			PhoneNumber = "1234567890",
			UserPassword = "password123"
		};

		_userManagerMock.Setup(x => x.FindByEmailAsync(registerRequest.Email))
			.ReturnsAsync((AppUser?)null);

		_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), registerRequest.UserPassword))
			.ReturnsAsync(IdentityResult.Success);

		_roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
			.ReturnsAsync(true); // Rola istnieje

		_userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
			.ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed to add role" }));

		_authLocalizerMock.Setup(x => x[AuthResourcesKeys.RegisterGlobalError])
			.Returns(new LocalizedString(AuthResourcesKeys.RegisterGlobalError, "User registration failed"));

		// Act
		var result = await _authService.RegisterAsync(registerRequest);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsRegisteredSuccessful, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("User registration failed"));
		});
	}

	[Test]
	public async Task RegisterNewEmployeeAsync_SuccessfulRegistration_ReturnsSuccessResponse()
	{
		// Arrange
		var newEmployeeRequest = new NewEmployeeRequestDto
		{
			Email = "employee@example.com",
			UserName = "newemployee",
			FirstName = "New",
			LastName = "Employee"
		};

		const string tempPassword = "tempPassword123";
		const string tempPasswordEncrypted = "tempPassword123Encrypted";

		_tempPasswordGeneratorServiceMock.Setup(x => x.GenerateTempPassword())
			.Returns(tempPassword);

		_encryptionServiceMock.Setup(x => x.Encrypt(tempPasswordEncrypted, It.IsAny<string>()))
			.Returns(tempPasswordEncrypted);

		_userManagerMock.Setup(x => x.FindByEmailAsync(newEmployeeRequest.Email))
			.ReturnsAsync((AppUser?)null);

		_userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), tempPassword))
			.ReturnsAsync(IdentityResult.Success);

		_roleManagerMock.Setup(x => x.RoleExistsAsync(It.IsAny<string>()))
		.ReturnsAsync(true); // Rola istnieje

		_userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
			.ReturnsAsync(IdentityResult.Success);

		_userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
			.ReturnsAsync([]);

		_dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		// Act
		var result = await _authService.RegisterNewEmployeeAsync(newEmployeeRequest);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.True);
			Assert.That(result.ErrorMessage, Is.Empty);
			Assert.That(result.EmployeeId, Is.Not.Empty);
		});
	}

	[Test]
	public async Task TempPasswordChangeAsync_UserNotFound_ReturnsError()
	{
		// Arrange
		var request = new TempPasswordChangeRequestDto { UserId = Guid.NewGuid(), Password = "newPassword123" };
		_userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
			.ReturnsAsync((AppUser?)null);

		_authLocalizerMock.Setup(l => l[AuthResourcesKeys.PasswordChangeGlobalError])
			.Returns(new LocalizedString(AuthResourcesKeys.PasswordChangeGlobalError, "Global Error"));

		// Act
		var result = await _authService.TempPasswordChangeAsync(request);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("Global Error"));
		});
	}

	[Test]
	public async Task TempPasswordChangeAsync_UserNotTempPassword_ReturnsError()
	{
		// Arrange
		var user = new AppUser { IsTempPassword = false };
		var request = new TempPasswordChangeRequestDto { UserId = Guid.NewGuid(), Password = "newPassword123" };
		_userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
			.ReturnsAsync(user);

		_authLocalizerMock.Setup(l => l[AuthResourcesKeys.PasswordChangeGlobalError])
			.Returns(new LocalizedString(AuthResourcesKeys.PasswordChangeGlobalError, "Global Error"));

		// Act
		var result = await _authService.TempPasswordChangeAsync(request);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("Global Error"));
		});
	}

	[Test]
	public async Task TempPasswordChangeAsync_ResetPasswordFails_ReturnsError()
	{
		// Arrange
		var user = new AppUser { IsTempPassword = true };
		var request = new TempPasswordChangeRequestDto { UserId = Guid.NewGuid(), Password = "newPassword123" };
		_userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
			.ReturnsAsync(user);

		_userManagerMock.Setup(um => um.GeneratePasswordResetTokenAsync(user))
			.ReturnsAsync("resetToken");

		_userManagerMock.Setup(um => um.ResetPasswordAsync(user, "resetToken", request.Password))
			.ReturnsAsync(IdentityResult.Failed());

		_authLocalizerMock.Setup(l => l[AuthResourcesKeys.PasswordChangeGlobalError])
			.Returns(new LocalizedString(AuthResourcesKeys.PasswordChangeGlobalError, "Global Error"));

		// Act
		var result = await _authService.TempPasswordChangeAsync(request);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("Global Error"));
		});
	}

	[Test]
	public async Task TempPasswordChangeAsync_SuccessfulPasswordChange_ReturnsSuccess()
	{
		// Arrange
		var user = new AppUser { IsTempPassword = true };
		var request = new TempPasswordChangeRequestDto { UserId = Guid.NewGuid(), Password = "newPassword123" };
		_userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
			.ReturnsAsync(user);

		_userManagerMock.Setup(um => um.GeneratePasswordResetTokenAsync(user))
			.ReturnsAsync("resetToken");

		_userManagerMock.Setup(um => um.ResetPasswordAsync(user, "resetToken", request.Password))
			.ReturnsAsync(IdentityResult.Success);

		_dbContextMock.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(1);

		_dbContextMock.Setup(db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(Mock.Of<IDbContextTransaction>());

		// Act
		var result = await _authService.TempPasswordChangeAsync(request);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.True);
			Assert.That(result.UserId, Is.EqualTo(user.Id));
		});
	}

	[Test]
	public async Task TempPasswordChangeAsync_ExceptionThrown_ReturnsError()
	{
		// Arrange
		var user = new AppUser { IsTempPassword = true };
		var request = new TempPasswordChangeRequestDto { UserId = Guid.NewGuid(), Password = "newPassword123" };
		_userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
			.ReturnsAsync(user);

		_userManagerMock.Setup(um => um.GeneratePasswordResetTokenAsync(user))
			.ReturnsAsync("resetToken");

		_userManagerMock.Setup(um => um.ResetPasswordAsync(user, "resetToken", request.Password))
			.ThrowsAsync(new Exception("Database error"));

		_authLocalizerMock.Setup(l => l[AuthResourcesKeys.PasswordChangeGlobalError])
			.Returns(new LocalizedString(AuthResourcesKeys.PasswordChangeGlobalError, "Global Error"));

		_dbContextMock.Setup(db => db.Database.BeginTransactionAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(Mock.Of<IDbContextTransaction>());

		// Act
		var result = await _authService.TempPasswordChangeAsync(request);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("Global Error"));
		});
	}

	#region Mocks
	private static Mock<AppDbContext> MockAppDbContext()
	{
		var options = new DbContextOptions<AppDbContext>();
		var mockContext = new Mock<AppDbContext>(options);

		// Mockowanie DatabaseFacade
		var mockDatabaseFacade = new Mock<DatabaseFacade>(mockContext.Object);
		mockContext.Setup(m => m.Database).Returns(mockDatabaseFacade.Object);

		// Mockowanie transakcji
		var mockTransaction = new Mock<IDbContextTransaction>();
		mockDatabaseFacade.Setup(m => m.BeginTransactionAsync(It.IsAny<CancellationToken>()))
			.ReturnsAsync(mockTransaction.Object);

		return mockContext;
	}

	private static Mock<UserManager<AppUser>> MockUserManager()
	{
		var userStoreMock = new Mock<IUserStore<AppUser>>();
		var optionsMock = new Mock<IOptions<IdentityOptions>>();
		var passwordHasherMock = new Mock<IPasswordHasher<AppUser>>();
		var userValidatorsMock = new List<IUserValidator<AppUser>> { new Mock<IUserValidator<AppUser>>().Object };
		var passwordValidatorsMock = new List<IPasswordValidator<AppUser>> { new Mock<IPasswordValidator<AppUser>>().Object };
		var keyNormalizerMock = new Mock<ILookupNormalizer>();
		var errorsMock = new Mock<IdentityErrorDescriber>();
		var servicesMock = new Mock<IServiceProvider>();
		var loggerMock = new Mock<ILogger<UserManager<AppUser>>>();

		return new Mock<UserManager<AppUser>>(
			userStoreMock.Object,
			optionsMock.Object,
			passwordHasherMock.Object,
			userValidatorsMock,
			passwordValidatorsMock,
			keyNormalizerMock.Object,
			errorsMock.Object,
			servicesMock.Object,
			loggerMock.Object);
	}

	private static Mock<RoleManager<IdentityRole>> MockRoleManager()
	{
		var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
		var roleValidatorsMock = new List<IRoleValidator<IdentityRole>> { new Mock<IRoleValidator<IdentityRole>>().Object };
		var keyNormalizerMock = new Mock<ILookupNormalizer>();
		var errorsMock = new Mock<IdentityErrorDescriber>();
		var loggerMock = new Mock<ILogger<RoleManager<IdentityRole>>>();

		return new Mock<RoleManager<IdentityRole>>(
			roleStoreMock.Object,
			roleValidatorsMock,
			keyNormalizerMock.Object,
			errorsMock.Object,
			loggerMock.Object);
	}

	private static IConfiguration MockConfiguration()
	{
		var inMemorySettings = new Dictionary<string, string?> {
			{ ConfigurationHelper.MessageBusConnectionString, "ConnectionString" },
			{ ConfigurationHelper.EmailTempPasswordQueue, "SectionValue" },
			{ ConfigurationHelper.TempPasswordEncryptionKey, "TempPasswordEncryptionKey"}
		};

		return new ConfigurationBuilder()
			.AddInMemoryCollection(inMemorySettings)
			.Build();
	}
	#endregion
}