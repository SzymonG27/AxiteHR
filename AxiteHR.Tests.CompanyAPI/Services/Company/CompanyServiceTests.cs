using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Services.Company;
using AxiteHR.Services.CompanyAPI.Services.Company.Impl;
using Moq;

namespace AxiteHR.Tests.CompanyAPI.Services.Company;

[TestFixture]
public class CompanyServiceTests
{
	private Mock<ICompanyRepository> _companyRepositoryMock;
	private CompanyService _companyService;

	[SetUp]
	public void SetUp()
	{
		_companyRepositoryMock = new Mock<ICompanyRepository>();

		_companyService = new CompanyService(_companyRepositoryMock.Object);
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
}