using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Services.CompanyPermission;
using AxiteHR.Services.CompanyAPI.Services.CompanyRole.Impl;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Moq;
using CompanyModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.Company;
using CompanyRoleModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.CompanyRole;
using CompanyUserModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.CompanyUser;

namespace AxiteHR.Tests.CompanyAPI.Services.CompanyRole
{
	[TestFixture]
	public class CompanyRoleServiceTests
	{
		private const int CompanyUserId = 1;
		private AppDbContext _dbContext;
		private CompanyRoleService _service;
		private Mock<ICompanyUserService> _companyUserService;
		private Mock<ICompanyPermissionService> _companyPermissionService;
		private Mock<IStringLocalizer<CompanyResources>> _companyLocalizer;
		private Mock<ILogger<CompanyRoleService>> _logger;

		[SetUp]
		public async Task Setup()
		{
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

			_companyUserService = new Mock<ICompanyUserService>();
			_companyPermissionService = new Mock<ICompanyPermissionService>();
			_companyLocalizer = new Mock<IStringLocalizer<CompanyResources>>();
			_logger = new Mock<ILogger<CompanyRoleService>>();

			_service = new CompanyRoleService(_dbContext, _companyUserService.Object, _companyPermissionService.Object, _companyLocalizer.Object, _logger.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_dbContext.Database.CloseConnection();
			_dbContext.Dispose();
		}

		private async Task SeedDatabase()
		{
			// Seed CompanyRoles
			CompanyModel company = new()
			{
				Id = 1,
				CompanyName = "Test"
			};
			await _dbContext.Companies.AddAsync(company);

			CompanyRoleModel companyRole1 = new() {
				Id = 1,
				RoleName = "Admin"
			};
			CompanyRoleModel companyRole2 = new()
			{
				Id = 2,
				RoleName = "Manager"
			};
			CompanyRoleModel companyRole3 = new()
			{
				Id = 3,
				RoleName = "NotVisibleRole"
			};
			await _dbContext.CompanyRoles.AddAsync(companyRole1);
			await _dbContext.CompanyRoles.AddAsync(companyRole2);
			await _dbContext.CompanyRoles.AddAsync(companyRole3);

			// Seed CompanyRoleCompanies
			CompanyRoleCompany companyRoleCompany1 = new() {
				Id = 1,
				CompanyRole = companyRole1,
				Company = company,
				IsVisible = true,
				IsMain = true
			};
			CompanyRoleCompany companyRoleCompany2 = new()
			{
				Id = 2,
				CompanyRole = companyRole2,
				Company = company,
				IsVisible = true,
				IsMain = false
			};
			CompanyRoleCompany companyRoleCompany3 = new()
			{
				Id = 3,
				CompanyRole = companyRole3,
				Company = company,
				IsVisible = false,
				IsMain = false
			};
			await _dbContext.CompanyRoleCompanies.AddAsync(companyRoleCompany1);
			await _dbContext.CompanyRoleCompanies.AddAsync(companyRoleCompany2);
			await _dbContext.CompanyRoleCompanies.AddAsync(companyRoleCompany3);

			CompanyUserModel companyUser = new()
			{
				Id = CompanyUserId,
				Company = company
			};
			await _dbContext.CompanyUsers.AddAsync(companyUser);

			// Seed CompanyUserRoles
			CompanyUserRole companyUserRole1 = new()
			{
				Id = 1,
				CompanyUser = companyUser,
				IsSupervisor = true,
				CompanyRoleCompany = companyRoleCompany1
			};
			CompanyUserRole companyUserRole2 = new()
			{
				Id = 2,
				CompanyUser = companyUser,
				CompanyRoleCompany = companyRoleCompany1
			};
			CompanyUserRole companyUserRole3 = new()
			{
				Id = 3,
				CompanyUser = companyUser,
				CompanyRoleCompany = companyRoleCompany2
			};
			CompanyUserRole companyUserRole4 = new()
			{
				Id = 4,
				CompanyUser = companyUser,
				CompanyRoleCompany = companyRoleCompany3
			};
			await _dbContext.CompanyUserRoles.AddAsync(companyUserRole1);
			await _dbContext.CompanyUserRoles.AddAsync(companyUserRole2);
			await _dbContext.CompanyUserRoles.AddAsync(companyUserRole3);
			await _dbContext.CompanyUserRoles.AddAsync(companyUserRole4);

			await _dbContext.SaveChangesAsync();
		}

		[Test]
		public async Task GetList_UserHasRoleListPermission_ShouldReturnPagedCompanyRoles()
		{
			// Arrange
			var requestDto = new CompanyRoleListRequestDto { CompanyId = 1, UserRequestedId = Guid.NewGuid() };
			var pagination = new Pagination { Page = 0, ItemsPerPage = 10 };

			_companyUserService.Setup(cus => cus.GetIdAsync(It.IsAny<int>(), It.IsAny<Guid>()))
				.ReturnsAsync(CompanyUserId);

			_companyPermissionService.Setup(cps => cps.IsCompanyUserHasAnyPermissionAsync(It.IsAny<int>(), It.IsAny<List<int>>()))
				.ReturnsAsync(true);

			// Act
			var result = (await _service.GetListAsync(requestDto, pagination)).ToList();

			// Assert
			Assert.That(result.Count, Is.EqualTo(2));
			Assert.That(result[0].EmployeesCount, Is.EqualTo(2));
		}

		[Test]
		public async Task GetList_UserHasNotRoleListPermission_ShouldReturnPagedCompanyRoles()
		{
			// Arrange
			var requestDto = new CompanyRoleListRequestDto { CompanyId = 1, UserRequestedId = Guid.NewGuid() };
			var pagination = new Pagination { Page = 0, ItemsPerPage = 10 };

			_companyUserService.Setup(cus => cus.GetIdAsync(It.IsAny<int>(), It.IsAny<Guid>()))
				.ReturnsAsync(CompanyUserId);

			_companyPermissionService.Setup(cps => cps.IsCompanyUserHasAnyPermissionAsync(It.IsAny<int>(), It.IsAny<List<int>>()))
				.ReturnsAsync(false);

			// Act
			var result = (await _service.GetListAsync(requestDto, pagination)).ToList();

			// Assert
			Assert.That(result.Count, Is.EqualTo(1));
			Assert.That(result[0].EmployeesCount, Is.EqualTo(2));
		}

		[Test]
		public async Task GetCountListAsync_UserHasRoleListPermission_ShouldReturnCorrectCount()
		{
			// Arrange
			var requestDto = new CompanyRoleListRequestDto { CompanyId = 1 };

			_companyUserService.Setup(cus => cus.GetIdAsync(It.IsAny<int>(), It.IsAny<Guid>()))
				.ReturnsAsync(CompanyUserId);

			_companyPermissionService.Setup(cps => cps.IsCompanyUserHasAnyPermissionAsync(It.IsAny<int>(), It.IsAny<List<int>>()))
				.ReturnsAsync(true);

			// Act
			var count = await _service.GetCountListAsync(requestDto);

			// Assert
			Assert.That(count, Is.EqualTo(2));
		}

		[Test]
		public async Task GetCountListAsync_UserHasNotRoleListPermission_ShouldReturnCorrectCount()
		{
			// Arrange
			var requestDto = new CompanyRoleListRequestDto { CompanyId = 1 };

			_companyUserService.Setup(cus => cus.GetIdAsync(It.IsAny<int>(), It.IsAny<Guid>()))
				.ReturnsAsync(CompanyUserId);

			_companyPermissionService.Setup(cps => cps.IsCompanyUserHasAnyPermissionAsync(It.IsAny<int>(), It.IsAny<List<int>>()))
				.ReturnsAsync(false);

			// Act
			var count = await _service.GetCountListAsync(requestDto);

			// Assert
			Assert.That(count, Is.EqualTo(1));
		}
	}
}
