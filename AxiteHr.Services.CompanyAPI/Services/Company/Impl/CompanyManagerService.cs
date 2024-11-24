using AutoMapper;
using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Const;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;
using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using CompanyUserModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.CompanyUser;

namespace AxiteHR.Services.CompanyAPI.Services.Company.Impl
{
	public class CompanyManagerService(AppDbContext dbContext,
		IMapper mapper,
		IStringLocalizer<CompanyResources> companyLocalizer,
		ILogger<CompanyManagerService> logger) : ICompanyManagerService
	{
		public async Task<NewCompanyReponseDto> NewCompanyCreateAsync(NewCompanyRequestDto newCompanyRequest)
		{
			NewCompanyReponseDto response = new();

			await using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				var newCompany = await AddNewCompany();
				var newCompanyUser = await AddCreatorCompanyUser(newCompany);
				await AddCreatorRoleAsync(newCompany, newCompanyUser, newCompanyRequest.CreatorId);
				await AddCreatorPermission(newCompanyUser);

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();

				response.IsSucceeded = true;
				response.ErrorMessage = string.Empty;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error while creating new company, params: {Params}", newCompanyRequest);

				await transaction.RollbackAsync();

				response.IsSucceeded = false;
				response.ErrorMessage = companyLocalizer[CompanyResourcesKeys.NewCompanyCreate_InternalError];
			}

			return response;

			async Task<Models.CompanyModels.Company> AddNewCompany()
			{
				var newCompany = mapper.Map<Models.CompanyModels.Company>(newCompanyRequest);
				newCompany.CompanyLevel = await dbContext.CompanyLevels.SingleAsync(x => x.Id == (int)CompanyLevelDictionary.Max10Emplyees);
				await dbContext.Companies.AddAsync(newCompany);

				return newCompany;
			}

			async Task<CompanyUserModel> AddCreatorCompanyUser(Models.CompanyModels.Company newCompany)
			{
				CompanyUserModel newCompanyUser = new()
				{
					Company = newCompany,
					UserId = newCompanyRequest.CreatorId,
					InsUserId = newCompanyRequest.CreatorId,
					InsDate = DateTime.UtcNow
				};
				await dbContext.CompanyUsers.AddAsync(newCompanyUser);

				return newCompanyUser;
			}

			async Task AddCreatorRoleAsync(Models.CompanyModels.Company newCompany, CompanyUserModel companyCreator, Guid insUserId)
			{
				var companyCreatorRole = await dbContext.CompanyRoles.SingleAsync(x => x.Id == (int)CompanyRoleDictionary.CompanyCreator);

				CompanyRoleCompany newCompanyRoleCompany = new()
				{
					Company = newCompany,
					CompanyRole = companyCreatorRole,
					IsMain = false,
					IsVisible = false
				};

				await dbContext.CompanyRoleCompanies.AddAsync(newCompanyRoleCompany);

				CompanyUserRole newCompanyUserRole = new()
				{
					CompanyUser = companyCreator,
					CompanyRoleCompany = newCompanyRoleCompany,
					InsDate = DateTime.UtcNow,
					InsUserId = insUserId
				};
				await dbContext.CompanyUserRoles.AddAsync(newCompanyUserRole);
			}

			async Task AddCreatorPermission(CompanyUserModel companyCreator)
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