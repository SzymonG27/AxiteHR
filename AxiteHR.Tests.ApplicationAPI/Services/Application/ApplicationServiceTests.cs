using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.ApplicationAPI.Data;
using AxiteHR.Services.ApplicationAPI.Models.Application;
using AxiteHR.Services.ApplicationAPI.Models.Application.Dto;
using AxiteHR.Services.ApplicationAPI.Models.Application.Enums;
using AxiteHR.Services.ApplicationAPI.Services.Application.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;

namespace AxiteHR.Tests.ApplicationAPI.Services.Application
{
	[TestFixture]
	public class ApplicationServiceTests
	{
		private AppDbContext _dbContext;
		private Mock<IStringLocalizer<ApplicationResources>> _localizerMock;
		private ApplicationService _applicationService;

		[SetUp]
		public void SetUp()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite("DataSource=:memory:")
			.Options;

			_dbContext = new AppDbContext(options);
			_dbContext.Database.OpenConnection();
			_dbContext.Database.EnsureCreated();

			_localizerMock = new Mock<IStringLocalizer<ApplicationResources>>();
			_applicationService = new ApplicationService(_dbContext, _localizerMock.Object);
		}

		[Test]
		public async Task CreateNewUserApplicationAsync_WhenApplicationIntersects_ReturnsFailedResponse()
		{
			// Arrange
			_dbContext.UserApplications.Add(new UserApplication
			{
				CompanyUserId = 1,
				ApplicationStatus = ApplicationStatus.New,
				ApplicationType = ApplicationType.Vacation,
				DateFrom = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
				DateTo = new DateTime(2024, 1, 10, 8, 0, 0, DateTimeKind.Unspecified),
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow
			});

			_dbContext.UserCompanyDaysOffs.Add(new UserCompanyDaysOff
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				DaysOff = 20
			});
			await _dbContext.SaveChangesAsync();

			var dto = new CreateApplicationRequestDto
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				PeriodFrom = new DateTime(2024, 1, 10, 8, 0, 0, DateTimeKind.Unspecified),
				PeriodTo = new DateTime(2024, 1, 12, 8, 0, 0, DateTimeKind.Unspecified)
			};

			_localizerMock.Setup(localizer => localizer[It.IsAny<string>()])
				.Returns(new LocalizedString("Error", "Application period intersects."));

			// Act
			var result = await _applicationService.CreateNewUserApplicationAsync(dto);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.IsSucceeded, Is.False);
				Assert.That(result.ErrorMessage, Is.EqualTo("Application period intersects."));
			});
		}

		[Test]
		public async Task CreateNewUserApplicationAsync_WhenNotEnoughDaysOffWithWeekend_ReturnsFailedResponse()
		{
			// Arrange
			_dbContext.UserApplications.Add(new UserApplication
			{
				CompanyUserId = 1,
				ApplicationStatus = ApplicationStatus.New,
				ApplicationType = ApplicationType.Vacation,
				DateFrom = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
				DateTo = new DateTime(2024, 1, 10, 8, 0, 0, DateTimeKind.Unspecified),
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow
			});

			_dbContext.UserCompanyDaysOffs.Add(new UserCompanyDaysOff
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				DaysOff = 2
			});
			await _dbContext.SaveChangesAsync();

			var dto = new CreateApplicationRequestDto
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				PeriodFrom = new DateTime(2024, 1, 11, 0, 0, 0, DateTimeKind.Unspecified),
				PeriodTo = new DateTime(2024, 1, 15, 8, 0, 0, DateTimeKind.Unspecified)
			};

			_localizerMock.Setup(localizer => localizer[It.IsAny<string>()])
				.Returns(new LocalizedString("Error", "Not enough days off."));

			// Act
			var result = await _applicationService.CreateNewUserApplicationAsync(dto);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.IsSucceeded, Is.False);
				Assert.That(result.ErrorMessage, Is.EqualTo("Not enough days off."));
			});
		}

		[Test]
		public async Task CreateNewUserApplicationAsync_WhenNotEnoughDaysOffWithoutWeekend_ReturnsFailedResponse()
		{
			// Arrange
			_dbContext.UserApplications.Add(new UserApplication
			{
				CompanyUserId = 1,
				ApplicationStatus = ApplicationStatus.New,
				ApplicationType = ApplicationType.Vacation,
				DateFrom = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
				DateTo = new DateTime(2024, 1, 7, 8, 0, 0, DateTimeKind.Unspecified),
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow
			});

			_dbContext.UserCompanyDaysOffs.Add(new UserCompanyDaysOff
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				DaysOff = 4.5M
			});
			await _dbContext.SaveChangesAsync();

			var dto = new CreateApplicationRequestDto
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				PeriodFrom = new DateTime(2024, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
				PeriodTo = new DateTime(2024, 1, 12, 7, 0, 0, DateTimeKind.Unspecified)
			};

			_localizerMock.Setup(localizer => localizer[It.IsAny<string>()])
				.Returns(new LocalizedString("Error", "Not enough days off."));

			// Act
			var result = await _applicationService.CreateNewUserApplicationAsync(dto);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.IsSucceeded, Is.False);
				Assert.That(result.ErrorMessage, Is.EqualTo("Not enough days off."));
			});
		}

		[Test]
		public async Task CreateNewUserApplicationAsync_WhenAllConditionsMet_ReturnsSuccessResponse()
		{
			// Arrange
			_dbContext.UserApplications.Add(new UserApplication
			{
				CompanyUserId = 1,
				ApplicationStatus = ApplicationStatus.New,
				ApplicationType = ApplicationType.Vacation,
				DateFrom = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
				DateTo = new DateTime(2024, 1, 7, 8, 0, 0, DateTimeKind.Unspecified),
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow
			});

			_dbContext.UserCompanyDaysOffs.Add(new UserCompanyDaysOff
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				DaysOff = 20
			});
			await _dbContext.SaveChangesAsync();

			var dto = new CreateApplicationRequestDto
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				PeriodFrom = new DateTime(2024, 1, 8, 0, 0, 0, DateTimeKind.Unspecified),
				PeriodTo = new DateTime(2024, 1, 12, 8, 0, 0, DateTimeKind.Unspecified)
			};

			_localizerMock.Setup(localizer => localizer[It.IsAny<string>()])
				.Returns(new LocalizedString("Success", "Application created successfully."));

			// Act
			var result = await _applicationService.CreateNewUserApplicationAsync(dto);

			// Assert
			Assert.That(result.IsSucceeded, Is.True);

			var userDaysOff = await _dbContext.UserCompanyDaysOffs.SingleAsync();
			Assert.That(userDaysOff.DaysOff, Is.EqualTo(15M));
		}

		[Test]
		public async Task CreateNewUserApplicationAsync_WhenExceptionOccurs_RollsBackTransactionAndReturnsError()
		{
			// Arrange
			_dbContext.UserApplications.Add(new UserApplication
			{
				CompanyUserId = 1,
				ApplicationStatus = ApplicationStatus.New,
				ApplicationType = ApplicationType.Vacation,
				DateFrom = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
				DateTo = new DateTime(2024, 1, 7, 8, 0, 0, DateTimeKind.Unspecified),
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow
			});

			_dbContext.UserApplications.Add(new UserApplication
			{
				CompanyUserId = 1,
				ApplicationStatus = ApplicationStatus.New,
				ApplicationType = ApplicationType.Vacation,
				DateFrom = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
				DateTo = new DateTime(2024, 1, 7, 8, 0, 0, DateTimeKind.Unspecified),
				InsUserId = Guid.NewGuid(),
				InsDate = DateTime.UtcNow,
				UpdUserId = Guid.NewGuid(),
				UpdDate = DateTime.UtcNow
			});

			_dbContext.UserCompanyDaysOffs.Add(new UserCompanyDaysOff
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				DaysOff = 20
			});
			await _dbContext.SaveChangesAsync();

			var dto = new CreateApplicationRequestDto
			{
				CompanyUserId = 1,
				ApplicationType = ApplicationType.Vacation,
				PeriodFrom = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
				PeriodTo = new DateTime(2024, 1, 10, 0, 0, 0, DateTimeKind.Unspecified)
			};

			_localizerMock.Setup(localizer => localizer[It.IsAny<string>()])
				.Returns(new LocalizedString("Error", "Internal server error."));

			// Act
			var result = await _applicationService.CreateNewUserApplicationAsync(dto);

			// Assert
			Assert.Multiple(() =>
			{
				Assert.That(result.IsSucceeded, Is.False);
				Assert.That(result.ErrorMessage, Is.EqualTo("Internal server error."));
			});
		}

		[TearDown]
		public void TearDown()
		{
			_dbContext.Database.EnsureDeleted();
			_dbContext.Dispose();
		}
	}
}