﻿using AutoMapper;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Services.Company.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using CompanyRoleModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.CompanyRole;

namespace AxiteHR.Tests.CompanyAPI.Services.Company;

[TestFixture]
public class CompanyCreatorServiceTests
{
	private AppDbContext _dbContext;
	private Mock<IMapper> _mapperMock;
	private Mock<IStringLocalizer<CompanyResources>> _localizerMock;
	private Mock<ILogger<CompanyManagerService>> _loggerMock;
	private CompanyManagerService _companyCreatorService;

	[SetUp]
	public async Task SetUp()
	{
		// Konfiguracja DbContext dla SQLite InMemory
		var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite("DataSource=:memory:")
			.Options;

		_dbContext = new AppDbContext(options)
		{
			SkipSeedData = true
		};
		await _dbContext.Database.OpenConnectionAsync();
		await _dbContext.Database.EnsureCreatedAsync();

		// Seed data
		await SeedDatabase();

		_mapperMock = new Mock<IMapper>();
		_localizerMock = new Mock<IStringLocalizer<CompanyResources>>();
		_loggerMock = new Mock<ILogger<CompanyManagerService>>();

		_companyCreatorService = new CompanyManagerService(
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

	private async Task SeedDatabase()
	{
		CompanyLevel companyLevel = new()
		{
			Id = (int)CompanyLevelDictionary.Max10Emplyees,
			MaxNumberOfWorkers = 10
		};
		await _dbContext.CompanyLevels.AddAsync(companyLevel);

		CompanyRoleModel companyRoleCreator = new()
		{
			Id = (int)CompanyRoleDictionary.CompanyCreator,
			RoleName = "Założyciel firmy",
			RoleNameEng = "Company creator"
		};
		await _dbContext.CompanyRoles.AddAsync(companyRoleCreator);

		CompanyPermission companyPermissionCreator = new()
		{
			Id = (int)PermissionDictionary.CompanyManager,
			PermissionName = "Company manager"
		};
		await _dbContext.CompanyPermissions.AddAsync(companyPermissionCreator);

		await _dbContext.SaveChangesAsync();
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

		var newCompany = new AxiteHR.Services.CompanyAPI.Models.CompanyModels.Company
		{
			Id = 1,
			CompanyName = request.CompanyName
		};

		_mapperMock.Setup(m => m.Map<AxiteHR.Services.CompanyAPI.Models.CompanyModels.Company>(It.IsAny<NewCompanyRequestDto>()))
			.Returns(newCompany);

		// Act
		var result = await _companyCreatorService.NewCompanyCreateAsync(request);

		// Assert
		await Assert.MultipleAsync(async () =>
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

		_mapperMock.Setup(m => m.Map<AxiteHR.Services.CompanyAPI.Models.CompanyModels.Company>(It.IsAny<NewCompanyRequestDto>()))
			.Throws(new Exception("Test Exception"));

		_localizerMock.Setup(localizer => localizer[It.IsAny<string>()])
				.Returns(new LocalizedString("Error", "Internal error."));

		// Act
		var result = await _companyCreatorService.NewCompanyCreateAsync(request);

		// Assert
		await Assert.MultipleAsync(async () =>
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