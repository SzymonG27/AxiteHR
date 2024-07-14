using AutoMapper;
using AxiteHr.Services.CompanyAPI.CompanyModels.Dto.Request;
using AxiteHr.Services.CompanyAPI.Data;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHr.Services.CompanyAPI.Models.CompanyModels.Dto.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AxiteHr.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyCreatorService(AppDbContext dbContext,
		IMapper mapper,
		ILogger<CompanyCreatorService> logger) : ICompanyCreatorService
	{
		public async Task<NewCompanyReponseDto> NewCompanyCreate(NewCompanyRequestDto newCompanyRequest)
		{
			NewCompanyReponseDto response = new();

			await using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				var newCompany = await AddNewCompany();
				var newCompanyUser = await AddCreatorCompanyUser(newCompany);
				await AddCreatorRoleAsync(newCompanyUser, newCompanyRequest.CreatorId);
				await AddCreatorPermission(newCompanyUser);

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();

				response.IsSucceeded = true;
				response.ErrorMessage = string.Empty;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error while creating new company");

				await transaction.RollbackAsync();

				response.IsSucceeded = false;
				response.ErrorMessage = ex.Message;
			}

			return response;

			async Task<Models.CompanyModels.Company> AddNewCompany()
			{
				var newCompany = mapper.Map<Models.CompanyModels.Company>(newCompanyRequest);
				newCompany.CompanyLevel = await dbContext.CompanyLevels.SingleAsync(x => x.Id == (int)CompanyLevelDictionary.Max10Emplyees);
				await dbContext.Companies.AddAsync(newCompany);

				return newCompany;
			}

			async Task<CompanyUser> AddCreatorCompanyUser(Models.CompanyModels.Company newCompany)
			{
				CompanyUser newCompanyUser = new()
				{
					Company = newCompany,
					UserId = newCompanyRequest.CreatorId,
					InsUserId = newCompanyRequest.CreatorId,
					InsDate = DateTime.UtcNow
				};
				await dbContext.CompanyUsers.AddAsync(newCompanyUser);

				return newCompanyUser;
			}

			async Task AddCreatorRoleAsync(CompanyUser companyCreator, Guid insUserId)
			{
				var companyCreatorRole = await dbContext.CompanyRoles.SingleAsync(x => x.Id == (int)CompanyRoleDictionary.CompanyCreator);
				CompanyUserRole newCompanyUserRole = new()
				{
					CompanyUser = companyCreator,
					CompanyRole = companyCreatorRole,
					InsDate = DateTime.UtcNow,
					InsUserId = insUserId
				};
				await dbContext.CompanyUserRoles.AddAsync(newCompanyUserRole);
			}

			async Task AddCreatorPermission(CompanyUser companyCreator)
			{
				var companyManagerPermission = await dbContext.CompanyPermissions.SingleAsync(x => x.Id == (int)PermissionDictionary.CompanyManager);
				CompanyUserPermission newCompanyUserPermission = new()
				{
					CompanyUser = companyCreator,
					CompanyPermission = companyManagerPermission,
				};
				await dbContext.CompanyUserPermissions.AddAsync(newCompanyUserPermission);
			}
		}
	}
}