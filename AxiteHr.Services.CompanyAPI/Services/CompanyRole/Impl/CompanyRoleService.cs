using AxiteHR.GlobalizationResources;
using AxiteHR.GlobalizationResources.Resources;
using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Helpers;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Infrastructure.AuthApi;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;
using AxiteHR.Services.CompanyAPI.Models.EmployeeModels.Dto;
using AxiteHR.Services.CompanyAPI.Services.CompanyPermission;
using AxiteHR.Services.CompanyAPI.Services.CompanyUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using CompanyRoleModel = AxiteHR.Services.CompanyAPI.Models.CompanyModels.CompanyRole;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyRole.Impl
{
	public class CompanyRoleService(
		AppDbContext dbContext,
		ICompanyUserService companyUserService,
		ICompanyPermissionService companyPermissionService,
		IAuthApiClient authApiClient,
		IStringLocalizer<CompanyResources> companyLocalizer,
		ILogger<CompanyRoleService> logger) : ICompanyRoleService
	{
		public async Task<IEnumerable<CompanyRoleListResponseDto>> GetListAsync(CompanyRoleListRequestDto requestDto, Pagination pagination)
		{
			if (pagination.ItemsPerPage <= 0)
			{
				pagination.ItemsPerPage = 10;
			}

			var companyUserId = await companyUserService.GetIdAsync(requestDto.CompanyId, requestDto.UserRequestedId);

			if (companyUserId == null)
			{
				logger.LogWarning("User was not in the company, but tried to get list of roles, UserId: {UserId}, CompanyId: {CompanyId}", requestDto.UserRequestedId, requestDto.CompanyId);
				return [];
			}

			var isUserCanSeeEntireList = await companyPermissionService.IsCompanyUserHasAnyPermissionAsync(companyUserId.Value, CompanyPermissionsHelper.CompanyRoleSeeEntireListPermissions);

			var query = dbContext.CompanyRoles
				.Join(dbContext.CompanyRoleCompanies,
					cr => cr.Id,
					crc => crc.CompanyRoleId,
					(cr, crc) => new { cr, crc })
				.GroupJoin(dbContext.CompanyUserRoles,
					x => x.crc.Id,
					cur => cur.CompanyRoleCompanyId,
					(x, cur) => new { x.cr, x.crc, cur })
				.SelectMany(
					x => x.cur.DefaultIfEmpty(),
					(x, companyUser) => new { x.cr, x.crc, CompanyUser = companyUser })
				.Where(x => x.crc.IsVisible && x.crc.CompanyId == requestDto.CompanyId);

			if (!isUserCanSeeEntireList)
			{
				query = query.Where(x => x.CompanyUser != null && x.CompanyUser.Id == companyUserId);
			}

			if (!string.IsNullOrEmpty(requestDto.RoleName))
			{
				query = query.Where(x => x.cr.RoleName.Contains(requestDto.RoleName) || x.cr.RoleNameEng.Contains(requestDto.RoleName));
			}

			return await query.GroupBy(x => new
			{
				x.cr.Id,
				CompanyRoleCompanyId = x.crc.Id,
				x.cr.RoleName,
				x.crc.IsMain
			})
				.OrderBy(x => x.Key.Id)
				.Skip(pagination.Page * pagination.ItemsPerPage)
				.Take(pagination.ItemsPerPage)
				.Select(x => new CompanyRoleListResponseDto
				{
					CompanyRoleId = x.Key.Id,
					CompanyRoleCompanyId = x.Key.CompanyRoleCompanyId,
					Name = x.Key.RoleName,
					IsMain = x.Key.IsMain,
					EmployeesCount = dbContext.CompanyUserRoles.Count(cu => cu.CompanyRoleCompanyId == x.Key.Id)
				})
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<int> GetCountListAsync(CompanyRoleListRequestDto requestDto)
		{
			var companyUserId = await companyUserService.GetIdAsync(requestDto.CompanyId, requestDto.UserRequestedId);

			if (companyUserId == null)
			{
				logger.LogWarning("User was not in the company, but tried to get count of roles, UserId: {UserId}, CompanyId: {CompanyId}", requestDto.UserRequestedId, requestDto.CompanyId);
				return 0;
			}

			var isUserCanSeeEntireList = await companyPermissionService.IsCompanyUserHasAnyPermissionAsync(companyUserId.Value, CompanyPermissionsHelper.CompanyRoleSeeEntireListPermissions);

			var query = dbContext.CompanyRoles
				.Join(dbContext.CompanyRoleCompanies,
					cr => cr.Id,
					crc => crc.CompanyRoleId,
					(cr, crc) => new { cr, crc })
				.Where(x => x.crc.IsVisible && x.crc.CompanyId == requestDto.CompanyId);

			if (!string.IsNullOrEmpty(requestDto.RoleName))
			{
				query = query.Where(x => x.cr.RoleName.Contains(requestDto.RoleName) || x.cr.RoleNameEng.Contains(requestDto.RoleName));
			}

			if (!isUserCanSeeEntireList)
			{
				return await query
					.Join(dbContext.CompanyUserRoles,
						x => x.crc.Id,
						cur => cur.CompanyRoleCompanyId,
						(x, cur) => new { x.cr, x.crc, cur })
					.Where(x => x.cur.Id == companyUserId)
					.CountAsync();

			}

			return await query.CountAsync();
		}

		public async Task<CompanyRoleCreatorResponseDto> CreateAsync(CompanyRoleCreatorRequestDto requestDto)
		{
			CompanyRoleCreatorResponseDto responseDto = new();

			await using var transaction = await dbContext.Database.BeginTransactionAsync();
			try
			{
				var companyUserId = await companyUserService.GetIdAsync(requestDto.CompanyId, requestDto.UserRequestedId)
					?? throw new UnauthorizedAccessException("User was not in the company, but tried to create new role");

				if (!await companyPermissionService.IsCompanyUserHasAnyPermissionAsync(companyUserId, CompanyPermissionsHelper.CompanyRoleCreatePermissions))
				{
					responseDto.IsSucceeded = false;
					responseDto.ErrorMessage = companyLocalizer[CompanyResourcesKeys.CompanyRoleCreate_UserDoesntHavePermissions];
					return responseDto;
				}

				var companyRole = await dbContext.CompanyRoles
					.FirstOrDefaultAsync(x => x.RoleName == requestDto.RoleNameTrimmed && x.RoleNameEng == requestDto.RoleNameEngTrimmed);

				CompanyRoleCompany? companyRoleCompany;

				if (companyRole == null)
				{
					companyRole = await CreateCompanyRoleAsync(requestDto);
					companyRoleCompany = await CreateCompanyRoleCompanyAsync(requestDto.CompanyId, companyRole);
				}
				else
				{
					if (await IsCompanyRoleHasRelationToCompanyAsync(requestDto.CompanyId, companyRole.Id))
					{
						responseDto.IsSucceeded = false;
						responseDto.ErrorMessage = companyLocalizer[CompanyResourcesKeys.CompanyRoleCreate_CompanyRoleCompanyExists];
						return responseDto;
					}

					companyRoleCompany = await CreateCompanyRoleCompanyAsync(requestDto.CompanyId, companyRole);
				}

				responseDto.IsSucceeded = true;

				await dbContext.SaveChangesAsync();
				await transaction.CommitAsync();

				responseDto.CompanyRoleId = companyRole.Id;
				responseDto.CompanyRoleCompanyId = companyRoleCompany.Id;

				return responseDto;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error while creating new company, params: {Params}", requestDto);

				await transaction.RollbackAsync();

				responseDto.IsSucceeded = false;
				responseDto.ErrorMessage = companyLocalizer[CompanyResourcesKeys.NewCompanyCreate_InternalError];
			}

			return responseDto;
		}

		public async Task<IEnumerable<CompanyUserDataDto>> GetListOfEmployeesToAttachAsync(CompanyRoleUserToAttachRequestDto requestDto, Pagination pagination, string bearerToken)
		{
			if (pagination.ItemsPerPage <= 0)
			{
				pagination.ItemsPerPage = 10;
			}

			var companyUserId = await companyUserService.GetIdAsync(requestDto.CompanyId, requestDto.UserRequestedId);

			if (companyUserId == null)
			{
				logger.LogWarning("User was not in the company, but tried to get list of employees to attach, UserId: {UserId}, CompanyId: {CompanyId}", requestDto.UserRequestedId, requestDto.CompanyId);
				return [];
			}

			var companyUserRelationList = await dbContext.CompanyUsers
				.Where(user => !dbContext.CompanyUserRoles
					.Where(role => role.CompanyUserId == user.Id)
					.Any(role =>
						dbContext.CompanyRoleCompanies
							.Any(crc => crc.Id == role.CompanyRoleCompanyId && crc.IsMain)))
				.OrderBy(x => x.Id)
				.Skip(pagination.Page * pagination.ItemsPerPage)
				.Take(pagination.ItemsPerPage)
				.Select(x => new CompanyUserUserRelation
				{
					CompanyUserId = x.Id,
					UserId = x.UserId
				})
				.AsNoTracking()
				.ToListAsync();

			if (companyUserRelationList.Count == 0)
			{
				return [];
			}

			return await authApiClient.GetUserDataListDtoAsync(companyUserRelationList, bearerToken);
		}

		public async Task<CompanyRoleAttachUserResponseDto> AttachUserAsync(CompanyRoleAttachUserRequestDto requestDto)
		{
			CompanyRoleAttachUserResponseDto responseDto = new();

			var isRequestedUserSupervisor = await dbContext.CompanyUserRoles
				.Where(x => x.CompanyRoleCompanyId == requestDto.CompanyRoleCompanyId
							&& x.CompanyUserId == requestDto.CompanyUserRequestedId
							&& x.IsSupervisor)
				.AnyAsync();

			var isRequestedUserHasAttachUserPermission = await companyPermissionService
				.IsCompanyUserHasAnyPermissionAsync(requestDto.CompanyUserRequestedId, CompanyPermissionsHelper.CompanyRoleAttachUserPermissions);

			if (!isRequestedUserSupervisor && !isRequestedUserHasAttachUserPermission)
			{
				responseDto.IsSucceeded = false;
				responseDto.ErrorMessage = companyLocalizer[CompanyResourcesKeys.AttachUserAsync_NoPermissionError];
				return responseDto;
			}

			//At the moment, one user = one main role in the company
			var isUserAttachedAsync = await dbContext.CompanyUserRoles
				.Include(x => x.CompanyRoleCompany)
				.AnyAsync(x => x.CompanyRoleCompanyId == requestDto.CompanyRoleCompanyId
							&& x.CompanyUserId == requestDto.CompanyUserToAttachId
							&& x.CompanyRoleCompany.IsMain);

			if (isUserAttachedAsync)
			{
				responseDto.IsSucceeded = false;
				responseDto.ErrorMessage = companyLocalizer[CompanyResourcesKeys.AttachUserAsync_UserAlreadyAttached];
				return responseDto;
			}

			var companyUserRole = new CompanyUserRole
			{
				CompanyRoleCompany = await dbContext.CompanyRoleCompanies.SingleAsync(x => x.Id == requestDto.CompanyRoleCompanyId),
				CompanyUser = await dbContext.CompanyUsers.SingleAsync(x => x.Id == requestDto.CompanyUserToAttachId),
				InsDate = DateTime.UtcNow,
				InsUserId = requestDto.UserRequestedId
			};

			await dbContext.CompanyUserRoles.AddAsync(companyUserRole);

			responseDto.IsSucceeded = true;
			responseDto.UserRoleId = companyUserRole.Id;
			return responseDto;
		}

		#region Private Methods
		private async Task<CompanyRoleModel> CreateCompanyRoleAsync(CompanyRoleCreatorRequestDto requestDto)
		{
			var companyRole = new CompanyRoleModel
			{
				RoleName = requestDto.RoleNameTrimmed,
				RoleNameEng = requestDto.RoleNameEngTrimmed
			};
			await dbContext.CompanyRoles.AddAsync(companyRole);

			return companyRole;
		}

		private async Task<CompanyRoleCompany> CreateCompanyRoleCompanyAsync(int companyId, CompanyRoleModel companyRole)
		{
			var companyRoleCompany = new CompanyRoleCompany
			{
				Company = await dbContext.Companies.SingleAsync(x => x.Id == companyId),
				CompanyRole = companyRole,
				IsVisible = true
			};
			await dbContext.CompanyRoleCompanies.AddAsync(companyRoleCompany);

			return companyRoleCompany;
		}

		private async Task<bool> IsCompanyRoleHasRelationToCompanyAsync(int companyId, int companyRoleId)
		{
			return await dbContext.CompanyRoleCompanies
				.Where(x => x.CompanyRoleId == companyRoleId && x.CompanyId == companyId)
				.AnyAsync();
		}
		#endregion
	}
}
