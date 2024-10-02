using AxiteHr.Services.CompanyAPI.Infrastructure;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHr.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHr.Services.CompanyAPI.Services.Company;
using AxiteHr.Services.CompanyAPI.Services.Company.Impl;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace AxiteHR.Tests.CompanyAPI.Services.Company;

[TestFixture]
public class CompanyServiceTests
{
	private Mock<IHttpClientFactory> _httpClientFactoryMock;
	private Mock<ICompanyRepository> _companyRepositoryMock;
	private CompanyService _companyService;

	[SetUp]
	public void SetUp()
	{
		_httpClientFactoryMock = new Mock<IHttpClientFactory>();
		_companyRepositoryMock = new Mock<ICompanyRepository>();

		_companyService = new CompanyService(_httpClientFactoryMock.Object, _companyRepositoryMock.Object);
	}

	[Test]
	public void GetCompanyList_ShouldReturnCompanyList()
	{
		// Arrange
		var userId = Guid.NewGuid();
		var expectedCompanyList = new List<CompanyListDto> { new() { Id = 1, CompanyName = "Company1" } };

		_companyRepositoryMock.Setup(x => x.GetCompanyList(userId)).Returns(expectedCompanyList);

		// Act
		var result = _companyService.GetCompanyList(userId);

		// Assert
		Assert.That(result, Is.EqualTo(expectedCompanyList));
	}

	[Test]
	public async Task GetCompanyUserViewDtoListAsync_ShouldReturnCompanyUserViewDtoList_WhenCompanyUserIdsAreNotEmpty()
	{
		// Arrange
		const int companyId = 1;
		var excludedUserId = Guid.NewGuid();
		var paginationInfo = new Pagination { ItemsPerPage = 10, Page = 1 };
		const string bearerToken = "fakeToken";

		var companyUserRelations = new List<CompanyUserUserRelation>
		{
			new() { CompanyUserId = 1, UserId = Guid.NewGuid() }
		};

		_companyRepositoryMock.Setup(x => x.GetCompanyUserUserRealtionListAsync(companyId, excludedUserId, paginationInfo))
			.ReturnsAsync(companyUserRelations);

		var expectedResponseContent = JsonSerializer.Serialize(new List<CompanyUserViewDto>
		{
			new() { UserId = companyUserRelations[0].UserId.ToString(), CompanyUserId = 1 }
		});

		var httpClientHandlerMock = new Mock<HttpMessageHandler>();
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
		var result = (await _companyService.GetCompanyUserViewDtoListAsync(companyId, excludedUserId, paginationInfo, bearerToken)).ToList();

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(result.Count, Is.EqualTo(1));
			Assert.That(result[0].CompanyUserId, Is.EqualTo(1));
		});
	}

	[Test]
	public async Task GetCompanyUserViewDtoListAsync_ShouldReturnEmptyList_WhenCompanyUserIdsAreEmpty()
	{
		// Arrange
		const int companyId = 1;
		var excludedUserId = Guid.NewGuid();
		var paginationInfo = new Pagination { ItemsPerPage = 10, Page = 1 };
		const string bearerToken = "fakeToken";

		_companyRepositoryMock.Setup(x => x.GetCompanyUserUserRealtionListAsync(companyId, excludedUserId, paginationInfo))
			.ReturnsAsync([]);

		// Act
		var result = await _companyService.GetCompanyUserViewDtoListAsync(companyId, excludedUserId, paginationInfo, bearerToken);

		// Assert
		Assert.That(result, Is.Empty);
	}

	[Test]
	public async Task GetCompanyUsersCountAsync_ShouldReturnCompanyUsersCount()
	{
		// Arrange
		const int companyId = 1;
		var excludedUserId = Guid.NewGuid();
		const int expectedCount = 5;

		_companyRepositoryMock.Setup(x => x.GetCompanyUsersCountAsync(companyId, excludedUserId)).ReturnsAsync(expectedCount);

		// Act
		var result = await _companyService.GetCompanyUsersCountAsync(companyId, excludedUserId);

		// Assert
		Assert.That(result, Is.EqualTo(expectedCount));
	}
}