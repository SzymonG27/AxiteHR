using AutoMapper;
using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Services.Company.Impl;
using AxiteHR.GlobalizationResources.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;

namespace AxiteHR.Tests.CompanyAPI.Services.Company;

[TestFixture]
public class CompanyCreatorServiceTests
{
	private AppDbContext _dbContext;
	private Mock<IMapper> _mapperMock;
	private Mock<IStringLocalizer<CompanyResources>> _localizerMock;
	private Mock<ILogger<CompanyCreatorService>> _loggerMock;
	private CompanyCreatorService _companyCreatorService;

	[SetUp]
	public void SetUp()
	{
		// Konfiguracja DbContext dla SQLite InMemory
		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite("DataSource=:memory:")
			.Options;

		_dbContext = new AppDbContext(options);
		_dbContext.Database.OpenConnection();
		_dbContext.Database.EnsureCreated();

		_mapperMock = new Mock<IMapper>();
		_localizerMock = new Mock<IStringLocalizer<CompanyResources>>();
		_loggerMock = new Mock<ILogger<CompanyCreatorService>>();

		_companyCreatorService = new CompanyCreatorService(
			_dbContext,
			_mapperMock.Object,
			_localizerMock.Object,
			_loggerMock.Object);
	}

	[TearDown]
	public void TearDown()
	{
		_dbContext.Database.EnsureDeleted();
		_dbContext.Dispose();
	}

	[Test]
	public async Task NewCompanyCreateAsync_ShouldCreateNewCompany_WhenRequestIsValid()
	{
		// Arrange
		var request = new NewCompanyRequestDto
		{
			CompanyName = "Test Company",
			CreatorId = Guid.NewGuid()
		};

		var newCompany = new AxiteHr.Services.CompanyAPI.Models.CompanyModels.Company
		{
			Id = 1,
			CompanyName = request.CompanyName
		};

		_mapperMock.Setup(m => m.Map<AxiteHr.Services.CompanyAPI.Models.CompanyModels.Company>(It.IsAny<NewCompanyRequestDto>()))
			.Returns(newCompany);

		// Act
		var result = await _companyCreatorService.NewCompanyCreateAsync(request);

		// Assert
		Assert.Multiple(async () =>
		{
			Assert.That(result.IsSucceeded, Is.True);
			Assert.That(result.ErrorMessage, Is.Empty);

			Assert.That(await _dbContext.Companies.CountAsync(), Is.EqualTo(1));
			Assert.That(await _dbContext.CompanyUsers.CountAsync(), Is.EqualTo(1));
			Assert.That(await _dbContext.CompanyUserRoles.CountAsync(), Is.EqualTo(1));
			Assert.That(await _dbContext.CompanyUserPermissions.CountAsync(), Is.EqualTo(1));
		});
	}

	[Test]
	public async Task NewCompanyCreateAsync_ShouldRollbackTransaction_WhenExceptionIsThrown()
	{
		// Arrange
		var request = new NewCompanyRequestDto
		{
			CompanyName = "Test Company",
			CreatorId = Guid.NewGuid()
		};

		_mapperMock.Setup(m => m.Map<AxiteHr.Services.CompanyAPI.Models.CompanyModels.Company>(It.IsAny<NewCompanyRequestDto>()))
			.Throws(new Exception("Test Exception"));

		_localizerMock.Setup(localizer => localizer[It.IsAny<string>()])
				.Returns(new LocalizedString("Error", "Internal error."));

		// Act
		var result = await _companyCreatorService.NewCompanyCreateAsync(request);

		// Assert
		Assert.Multiple(async () =>
		{
			Assert.That(result.IsSucceeded, Is.False);
			Assert.That(result.ErrorMessage, Is.EqualTo("Internal error."));

			Assert.That(await _dbContext.Companies.CountAsync(), Is.EqualTo(0));
			Assert.That(await _dbContext.CompanyUsers.CountAsync(), Is.EqualTo(0));
			Assert.That(await _dbContext.CompanyUserRoles.CountAsync(), Is.EqualTo(0));
			Assert.That(await _dbContext.CompanyUserPermissions.CountAsync(), Is.EqualTo(0));
		});
	}
}