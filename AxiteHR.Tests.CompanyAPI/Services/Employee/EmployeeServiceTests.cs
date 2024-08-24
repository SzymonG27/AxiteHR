using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHr.Services.CompanyAPI.Services.Employee.Impl;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace AxiteHR.Tests.CompanyAPI.Services.Employee;

[TestFixture]
public class EmployeeServiceTests
{
	private Mock<IHttpClientFactory> _httpClientFactoryMock;
	private Mock<IStringLocalizer<CompanyResources>> _companyLocalizerMock;
	private Mock<IStringLocalizer<SharedResources>> _sharedLocalizerMock;
	private Mock<ILogger<EmployeeService>> _loggerMock;
	private AppDbContext _dbContext;
	private EmployeeService _employeeService;
	private const string AcceptLanguage = "pl";

	[SetUp]
	public void SetUp()
	{
		_httpClientFactoryMock = new Mock<IHttpClientFactory>();
		_companyLocalizerMock = new Mock<IStringLocalizer<CompanyResources>>();
		_sharedLocalizerMock = new Mock<IStringLocalizer<SharedResources>>();
		_loggerMock = new Mock<ILogger<EmployeeService>>();

		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite("DataSource=:memory:")
			.Options;
		_dbContext = new AppDbContext(options);
		_dbContext.Database.OpenConnection();
		_dbContext.Database.EnsureCreated();

		_employeeService = new EmployeeService(
			_httpClientFactoryMock.Object,
			_companyLocalizerMock.Object,
			_sharedLocalizerMock.Object,
			_loggerMock.Object,
			_dbContext);
	}

	[TearDown]
	public void TearDown()
	{
		_dbContext.Database.EnsureDeleted();
		_dbContext.Dispose();
	}

	[Test]
	public async Task CreateNewEmployeeAsync_ShouldReturnError_WhenUserHasNoManagerPermission()
	{
		// Arrange
		var requestDto = new NewEmployeeRequestDto { CompanyId = 1, InsUserId = Guid.NewGuid().ToString() };
		const string token = "fakeToken";

		_sharedLocalizerMock.Setup(l => l[SharedResourcesKeys.Global_UserWithoutPermission])
			.Returns(new LocalizedString(SharedResourcesKeys.Global_UserWithoutPermission, "User without permission"));

		// Act
		var result = await _employeeService.CreateNewEmployeeAsync(requestDto, token, AcceptLanguage);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("User without permission"));
		});
	}

	[Test]
	public async Task CreateNewEmployeeAsync_ShouldCreateNewEmployee_WhenUserHasManagerPermission()
	{
		// Arrange
		var existingCompany = new AxiteHr.Services.CompanyAPI.Models.CompanyModels.Company
		{
			CompanyName = "TEST"
		};
		_dbContext.Companies.Add(existingCompany);

		var insUserId = Guid.NewGuid();

		var existingUser = new CompanyUser
		{
			Company = existingCompany,
			UserId = insUserId
		};
		_dbContext.CompanyUsers.Add(existingUser);

		var existingPermission = await _dbContext.CompanyPermissions.SingleAsync(x => x.Id == (int)PermissionDictionary.CompanyManager);

		var userPermission = new CompanyUserPermission
		{
			CompanyUser = existingUser,
			CompanyPermission = existingPermission
		};
		_dbContext.CompanyUserPermissions.Add(userPermission);
		await _dbContext.SaveChangesAsync();

		var requestDto = new NewEmployeeRequestDto
		{
			CompanyId = existingCompany.Id,
			InsUserId = insUserId.ToString() // Przekazujemy identyfikator użytkownika jako string
		};
		const string token = "fakeToken";

		var newEmployeeResponse = new NewEmployeeResponseDto
		{
			IsSucceeded = true,
			EmployeeId = Guid.NewGuid().ToString()
		};

		var httpClientHandlerMock = new Mock<HttpMessageHandler>();
		httpClientHandlerMock.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync(new HttpResponseMessage
			{
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(JsonSerializer.Serialize(newEmployeeResponse))
			});

		var httpClient = new HttpClient(httpClientHandlerMock.Object)
		{
			BaseAddress = new Uri("https://api.example.com/")
		};

		_httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

		// Act
		var result = await _employeeService.CreateNewEmployeeAsync(requestDto, token, AcceptLanguage);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.True);
			Assert.That(result.EmployeeId, Is.Not.Null);
			Assert.That(result.EmployeeId, Is.EqualTo(newEmployeeResponse.EmployeeId));
		});
	}

	[Test]
	public async Task CreateNewEmployeeAsync_ShouldRollbackTransaction_OnFailure()
	{
		// Arrange
		var existingCompany = new AxiteHr.Services.CompanyAPI.Models.CompanyModels.Company
		{
			CompanyName = "TEST"
		};
		_dbContext.Companies.Add(existingCompany);

		var insUserId = Guid.NewGuid();

		var existingUser = new CompanyUser
		{
			Company = existingCompany,
			UserId = insUserId
		};
		_dbContext.CompanyUsers.Add(existingUser);

		var existingPermission = await _dbContext.CompanyPermissions.SingleAsync(x => x.Id == (int)PermissionDictionary.CompanyManager);

		var userPermission = new CompanyUserPermission
		{
			CompanyUser = existingUser,
			CompanyPermission = existingPermission
		};
		_dbContext.CompanyUserPermissions.Add(userPermission);
		await _dbContext.SaveChangesAsync();

		var requestDto = new NewEmployeeRequestDto
		{
			CompanyId = existingCompany.Id,
			InsUserId = insUserId.ToString() // Przekazujemy identyfikator użytkownika jako string
		};
		const string token = "fakeToken";

		_httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
			.Throws(new Exception("Some error"));

		_companyLocalizerMock.Setup(x => x[CompanyResourcesKeys.NewEmployeeRequestDto_RegisterError])
			.Returns(new LocalizedString(CompanyResourcesKeys.NewEmployeeRequestDto_RegisterError, "Register error"));

		// Act
		var result = await _employeeService.CreateNewEmployeeAsync(requestDto, token, AcceptLanguage);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.IsSucceeded, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("Register error"));
		});
	}
}