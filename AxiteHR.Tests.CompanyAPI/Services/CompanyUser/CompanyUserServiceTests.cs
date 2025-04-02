using AxiteHR.Integration.Cache.Redis;
using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser.Impl;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using CompanyModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.Company;
using CompanyUserModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.CompanyUser;

namespace AxiteHR.Tests.CompanyAPI.Services.CompanyUser
{
	[TestFixture]
	public class CompanyUserServiceTests
	{
		private AppDbContext _dbContext;
		private Mock<IHttpClientFactory> _httpClientFactoryMock;
		private Mock<IRedisCacheService> _redisCacheServiceMock;

		private CompanyUserService _companyUserService;

		[SetUp]
		public void SetUp()
		{
			_httpClientFactoryMock = new Mock<IHttpClientFactory>();
			_redisCacheServiceMock = new Mock<IRedisCacheService>();

			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseSqlite("DataSource=:memory:")
				.Options;
			_dbContext = new AppDbContext(options)
			{
				SkipSeedData = true
			};
			_dbContext.Database.OpenConnection();
			_dbContext.Database.EnsureCreated();

			_companyUserService = new CompanyUserService(
				_dbContext,
				_httpClientFactoryMock.Object,
				_redisCacheServiceMock.Object);
		}

		[TearDown]
		public void TearDown()
		{
			_dbContext.Database.EnsureDeleted();
			_dbContext.Dispose();
		}

		[Test]
		public async Task GetCompanyUserViewDtoListAsync_ShouldReturnCompanyUserViewDtoList_WhenCompanyUserIdsAreNotEmpty()
		{
			// Arrange
			const int companyId = 1;
			var excludedUserId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var paginationInfo = new Pagination { ItemsPerPage = 10, Page = 0 };
			const string bearerToken = "fakeToken";

			var company = new CompanyModel
			{
				Id = 1,
				CompanyName = "Test",
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow,
			};
			await _dbContext.Companies.AddAsync(company);

			var companyUser = new CompanyUserModel
			{
				Id = 1,
				UserId = userId,
				Company = company
			};
			await _dbContext.CompanyUsers.AddAsync(companyUser);

			await _dbContext.SaveChangesAsync();

			List<CompanyUserUserRelation> companyUserRelations =
			[
				new() { CompanyUserId = 1, UserId = userId }
			];

			string expectedResponseContent = JsonSerializer.Serialize(new List<CompanyUserViewDto>
			{
				new() { UserId = companyUserRelations[0].UserId.ToString(), CompanyUserId = 1 }
			});

			Mock<HttpMessageHandler> httpClientHandlerMock = new();
			httpClientHandlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(expectedResponseContent)
				});

			var httpClient = new HttpClient(httpClientHandlerMock.Object)
			{
				BaseAddress = new Uri("https://api.example.com/")  // Ustawienie BaseAddress
			};
			_httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

			// Act
			List<CompanyUserViewDto> result = (await _companyUserService.GetCompanyUserViewDtoListAsync(companyId, excludedUserId, paginationInfo, bearerToken)).ToList();

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result, Has.Count.EqualTo(1));
				Assert.That(result[0].CompanyUserId, Is.EqualTo(1));
			});
		}

		[Test]
		public async Task GetCompanyUserViewDtoListAsync_WhenCompanyUserIdsAreNotEmptyButPaginationAfterCompanyUsers_ShouldReturnEmptyList()
		{
			// Arrange
			const int companyId = 1;
			var excludedUserId = Guid.NewGuid();
			var userId = Guid.NewGuid();
			var paginationInfo = new Pagination { ItemsPerPage = 2, Page = 1 };
			const string bearerToken = "fakeToken";

			var company = new CompanyModel
			{
				Id = 1,
				CompanyName = "Test",
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow,
			};
			await _dbContext.Companies.AddAsync(company);

			var companyUser = new CompanyUserModel
			{
				Id = 1,
				UserId = userId,
				Company = company
			};
			await _dbContext.CompanyUsers.AddAsync(companyUser);

			await _dbContext.SaveChangesAsync();

			List<CompanyUserUserRelation> companyUserRelations =
			[
				new() { CompanyUserId = 1, UserId = userId }
			];

			string expectedResponseContent = JsonSerializer.Serialize(new List<CompanyUserViewDto>
			{
				new() { UserId = companyUserRelations[0].UserId.ToString(), CompanyUserId = 1 }
			});

			Mock<HttpMessageHandler> httpClientHandlerMock = new();
			httpClientHandlerMock.Protected()
				.Setup<Task<HttpResponseMessage>>(
					"SendAsync",
					ItExpr.IsAny<HttpRequestMessage>(),
					ItExpr.IsAny<CancellationToken>())
				.ReturnsAsync(new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = new StringContent(expectedResponseContent)
				});

			var httpClient = new HttpClient(httpClientHandlerMock.Object)
			{
				BaseAddress = new Uri("https://api.example.com/")  // Ustawienie BaseAddress
			};
			_httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

			// Act
			List<CompanyUserViewDto> result = (await _companyUserService.GetCompanyUserViewDtoListAsync(companyId, excludedUserId, paginationInfo, bearerToken)).ToList();

			// Assert
			Assert.That(result, Is.Empty);
		}

		[Test]
		public async Task GetCompanyUserViewDtoListAsync_ShouldReturnEmptyList_WhenCompanyUserIdsAreEmpty()
		{
			// Arrange
			const int companyId = 1;
			var excludedUserId = Guid.NewGuid();
			var paginationInfo = new Pagination { ItemsPerPage = 10, Page = 0 };
			const string bearerToken = "fakeToken";

			// Act
			var result = await _companyUserService.GetCompanyUserViewDtoListAsync(companyId, excludedUserId, paginationInfo, bearerToken);

			// Assert
			Assert.That(result, Is.Empty);
		}

		[Test]
		public async Task GetCompanyUsersCountAsync_ShouldReturnCompanyUsersCount()
		{
			// Arrange
			const int companyId = 1;
			var excludedUserId = Guid.NewGuid();
			const int expectedCount = 2;

			var company = new CompanyModel
			{
				Id = 1,
				CompanyName = "Test",
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow,
			};
			await _dbContext.Companies.AddAsync(company);

			var companyUser1 = new CompanyUserModel
			{
				Id = 1,
				UserId = Guid.NewGuid(),
				Company = company
			};
			await _dbContext.CompanyUsers.AddAsync(companyUser1);

			var companyUser2 = new CompanyUserModel
			{
				Id = 2,
				UserId = Guid.NewGuid(),
				Company = company
			};
			await _dbContext.CompanyUsers.AddAsync(companyUser2);

			await _dbContext.SaveChangesAsync();

			// Act
			var result = await _companyUserService.GetCompanyUsersCountAsync(companyId, excludedUserId);

			// Assert
			Assert.That(result, Is.EqualTo(expectedCount));
		}
	}
}
