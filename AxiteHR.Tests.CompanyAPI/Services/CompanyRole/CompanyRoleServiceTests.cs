using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Services.CompanyRole.Impl;
using Microsoft.EntityFrameworkCore;
using CompanyModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.Company;
using CompanyRoleModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.CompanyRole;

namespace AxiteHR.Tests.CompanyAPI.Services.CompanyRole
{
	[TestFixture]
	public class CompanyRoleServiceTests
	{
		private AppDbContext _dbContext;
		private CompanyRoleService _service;

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

			_service = new CompanyRoleService(_dbContext);
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

			await _dbContext.SaveChangesAsync();

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

			await _dbContext.SaveChangesAsync();

			// Seed CompanyUserRoles
			CompanyUserRole companyUserRole1 = new()
			{
				Id = 1,
				CompanyRoleCompany = companyRoleCompany1
			};
			CompanyUserRole companyUserRole2 = new()
			{
				Id = 2,
				CompanyRoleCompany = companyRoleCompany1
			};
			CompanyUserRole companyUserRole3 = new()
			{
				Id = 3,
				CompanyRoleCompany = companyRoleCompany2
			};
			CompanyUserRole companyUserRole4 = new()
			{
				Id = 4,
				CompanyRoleCompany = companyRoleCompany3
			};
			await _dbContext.CompanyUserRoles.AddAsync(companyUserRole1);
			await _dbContext.CompanyUserRoles.AddAsync(companyUserRole2);
			await _dbContext.CompanyUserRoles.AddAsync(companyUserRole3);
			await _dbContext.CompanyUserRoles.AddAsync(companyUserRole4);
			await _dbContext.SaveChangesAsync();
		}

		[Test]
		public void GetList_ShouldReturnPagedCompanyRoles()
		{
			// Arrange
			var requestDto = new CompanyRoleListRequestDto { CompanyId = 1 };
			var pagination = new Pagination { Page = 0, ItemsPerPage = 10 };

			// Act
			var result = _service.GetList(requestDto, pagination).ToList();

			// Assert
			Assert.That(result.Count, Is.EqualTo(2));
			Assert.That(result[0].EmployeesCount, Is.EqualTo(2));
		}

		[Test]
		public async Task GetCountListAsync_ShouldReturnCorrectCount()
		{
			// Arrange
			var requestDto = new CompanyRoleListRequestDto { CompanyId = 1 };

			// Act
			var count = await _service.GetCountListAsync(requestDto);

			// Assert
			Assert.That(count, Is.EqualTo(2));
		}
	}
}
