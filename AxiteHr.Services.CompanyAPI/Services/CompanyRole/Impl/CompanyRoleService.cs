using AxiteHR.Services.CompanyAPI.Data;
using AxiteHR.Services.CompanyAPI.Infrastructure;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Request;
using AxiteHR.Services.CompanyAPI.Models.CompanyModels.Dto.Response;
using Microsoft.EntityFrameworkCore;

namespace AxiteHR.Services.CompanyAPI.Services.CompanyRole.Impl
{
	public class CompanyRoleService(AppDbContext dbContext) : ICompanyRoleService
	{
		public IEnumerable<CompanyRoleListResponseDto> GetList(CompanyRoleListRequestDto requestDto, Pagination pagination)
		{
			if (pagination.ItemsPerPage <= 0)
			{
				pagination.ItemsPerPage = 10;
			}

			return [.. dbContext.CompanyRoles
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
				.Where(x => x.crc.IsVisible && x.crc.CompanyId == requestDto.CompanyId)
				.GroupBy(x => new { x.cr.Id, x.cr.RoleName, x.crc.IsMain })
				.OrderBy(x => x.Key.Id)
				.Skip(pagination.Page * pagination.ItemsPerPage)
				.Take(pagination.ItemsPerPage)
				.Select(x => new CompanyRoleListResponseDto
				{
					CompanyRoleId = x.Key.Id,
					Name = x.Key.RoleName,
					IsMain = x.Key.IsMain,
					EmployeesCount = x.Count(y => y.CompanyUser != null)
				})
				.AsNoTracking()];
		}

		public async Task<int> GetCountListAsync(CompanyRoleListRequestDto requestDto)
		{
			return await dbContext.CompanyRoles
				.Join(dbContext.CompanyRoleCompanies,
					cr => cr.Id,
					crc => crc.CompanyRoleId,
					(cr, crc) => new { cr, crc })
				.Where(x => x.crc.IsVisible && x.crc.CompanyId == requestDto.CompanyId)
				.CountAsync();
		}
	}
}
